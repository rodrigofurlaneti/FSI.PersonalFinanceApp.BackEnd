using FSI.PersonalFinanceApp.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FSI.PersonalFinanceApp.Api.Middlewares
{
    public sealed class ProblemDetailsExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProblemDetailsExceptionMiddleware> _log;

        public ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> log)
            => (_next, _log) = (next, log);

        public async Task Invoke(HttpContext http)
        {
            try
            {
                await _next(http);
            }
            catch (ValidationException ex)
            {
                await Write(http, 400, "Validation failed", "VALIDATION_ERROR", ex.Message, pb =>
                {
                    pb.Extensions["errors"] = ex.Errors;
                });
            }
            catch (NotFoundException ex)
            {
                await Write(http, 404, "Resource not found", "NOT_FOUND", ex.Message);
            }
            catch (ConflictException ex)
            {
                await Write(http, 409, "Conflict", "CONFLICT", ex.Message);
            }
            catch (UnauthorizedAppException ex)
            {
                await Write(http, 401, "Unauthorized", "UNAUTHORIZED", ex.Message);
            }
            catch (ForbiddenAppException ex)
            {
                await Write(http, 403, "Forbidden", "FORBIDDEN", ex.Message);
            }
            catch (RateLimitExceededException ex)
            {
                http.Response.Headers.RetryAfter = "60";
                await Write(http, 429, "Too Many Requests", "RATE_LIMIT", ex.Message);
            }
            catch (OperationCanceledException) when (http.RequestAborted.IsCancellationRequested)
            {
                // cliente cancelou; se não usa 499, pode responder 408
                await Write(http, 499, "Client Closed Request", "CLIENT_CANCELED", "Requisição cancelada pelo cliente.");
            }
            catch (TimeoutException ex)
            {
                await Write(http, 504, "Upstream timeout", "TIMEOUT", ex.Message);
            }
            catch (SqlException ex)
            {
                await WriteDbProblem(http, ex, false);
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("obtaining a connection from the pool", StringComparison.OrdinalIgnoreCase))
            {
                await Write(http, 503, "Connection pool exhausted", "DB_POOL_EXHAUSTED", "Limite de conexões atingido.", logException: ex);
            }
            catch (Exception ex)
            {
                await Write(http, 500, "Unexpected error", "UNEXPECTED", "Ocorreu um erro ao processar a requisição.", logException: ex);
            }
        }

        private static (int Status, string Title, string Reason, string Detail, bool Transient, string Hint) MapSql(SqlException ex)
        {
            return ex.Number switch
            {
                // Credencial / login
                18456 => (503, "DB auth failed", "DB_AUTH_FAILED",
                          "Falha de autenticação na dependência de dados.",
                          Transient: false,
                          Hint: "Verifique usuário/senha ou políticas de expiração."),
                // Banco indisponível / não pode abrir
                4060 => (503, "Database not open", "DB_UNAVAILABLE",
                         "Não foi possível abrir o banco configurado.",
                         Transient: false,
                         Hint: "Confirme o nome do DB, permissões e disponibilidade."),
                17142 => (503, "SQL Server paused", "DB_UNAVAILABLE",
                          "Instância de dados pausada/sem aceitar conexões.",
                          Transient: true,
                          Hint: "Aguarde a instância voltar; monitore serviço/CPU."),
                // Azure SQL throttling/erros transitórios
                40197 or 40501 or 40613 or 10928 or 10929 => (503, "Throttling/Transient error", "DB_TRANSIENT",
                          "A dependência de dados está limitando ou se recuperando.",
                          Transient: true,
                          Hint: "Aplique retry exponencial (Polly) e health check."),
                // Timeout
                -2 or 258 or 121 => (504, "Database timeout", "DB_TIMEOUT",
                          "Tempo excedido ao executar operação no banco.",
                          Transient: true,
                          Hint: "Revise índices/planos; aumente CommandTimeout se necessário."),
                // Permissão de objeto
                229 => (500, "Permission denied on object", "DB_PERMISSION_DENIED",
                        "Erro interno de permissão no schema de dados.",
                        Transient: false,
                        Hint: "Garanta GRANT/ROLE corretos no objeto acessado."),
                // Concorrência
                1205 or 1222 => (409, "Concurrency conflict", "DB_CONCURRENCY_CONFLICT",
                        "Conflito de bloqueio/concorrência no banco.",
                        Transient: true,
                        Hint: "Implemente retry/backoff e reduza escopo de transações."),
                // Unicidade
                2627 or 2601 => (409, "Unique constraint violation", "DB_UNIQUE_VIOLATION",
                        "Registro já existe (violação de unicidade).",
                        Transient: false,
                        Hint: "Valide duplicidade antes de inserir/atualizar."),
                // FK
                547 => (409, "Foreign key constraint", "DB_FK_CONSTRAINT",
                        "Restrição de integridade referencial violada.",
                        Transient: false,
                        Hint: "Garanta ordem de operações e integridade dos dados."),
                // Dados inválidos
                245 or 8152 => (422, "Invalid data for database", "DB_DATA_INVALID",
                        "Dados com tipo/tamanho inválido para o schema.",
                        Transient: false,
                        Hint: "Normalize tipos e tamanhos (ex.: truncamento de string)."),
                // Objeto/coluna inexistente
                208 or 207 => (500, "Schema mismatch", "DB_SCHEMA_MISMATCH",
                        "Objeto/coluna não encontrado(a) no banco.",
                        Transient: false,
                        Hint: "Sincronize migrações/DDL com a versão do app."),
                // Genérico
                _ => (503, "Database error", "DB_GENERIC",
                      "Falha ao acessar a dependência de dados.",
                      Transient: true,
                      Hint: "Verifique logs e conectividade com a instância.")
            };
        }

        private async Task WriteDbProblem(HttpContext http, SqlException ex, bool exposeDiagnostics)
        {
            var (status, title, reason, detail, transient, hint) = MapSql(ex);

            var pd = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = http.TraceIdentifier,
                Type = "about:blank"
            };

            // Extensões "seguras" (aparecem sempre)
            pd.Extensions["reason"] = reason;             // código estável p/ clientes
            pd.Extensions["correlationId"] = http.TraceIdentifier;
            pd.Extensions["transient"] = transient;       // cliente pode decidir retry
            pd.Extensions["hint"] = hint;                 // dica curta de ação

            // Diagnóstico opcional (apenas se habilitado por config/ambiente)
            if (exposeDiagnostics)
            {
                pd.Extensions["provider"] = "sqlserver";
                pd.Extensions["dbError"] = new
                {
                    ex.Number,
                    ex.State,
                    ex.Class,
                    ex.Server,     // cuidado: não coloque connection string
                    ex.Procedure,
                    ex.LineNumber
                };
            }

            // Cabeçalho útil para erros transitórios
            if (transient && status is 503 or 504)
                http.Response.Headers.RetryAfter = "10";

            _log.LogError(ex, "DB error mapped to {Status} {Reason} TraceId={TraceId}", status, reason, http.TraceIdentifier);

            http.Response.StatusCode = status;
            http.Response.ContentType = "application/problem+json";
            await http.Response.WriteAsJsonAsync(pd);
        }

        private async Task Write(HttpContext http, int status, string title, string reason, string detail,
                                 Action<ProblemDetails>? enrich = null, Exception? logException = null)
        {
            if (logException != null)
                _log.LogError(logException, "{Title} ({Reason}) TraceId={TraceId}", title, reason, http.TraceIdentifier);
            else
                _log.LogWarning("{Title} ({Reason}) TraceId={TraceId}", title, reason, http.TraceIdentifier);

            var pb = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = http.TraceIdentifier,
                Type = "about:blank"
            };
            pb.Extensions["reason"] = reason;
            pb.Extensions["correlationId"] = http.TraceIdentifier;
            enrich?.Invoke(pb);

            http.Response.StatusCode = status;
            http.Response.ContentType = "application/problem+json";
            await http.Response.WriteAsJsonAsync(pb);
        }
    }
}
