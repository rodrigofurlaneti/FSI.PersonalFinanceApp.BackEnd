using AutoMapper;
using FSI.PersonalFinanceApp.Api.Middlewares;
using FSI.PersonalFinanceApp.Application.DependencyInjection; // ✅ Novo local correto
using FSI.PersonalFinanceApp.Application.Interfaces;
using FSI.PersonalFinanceApp.Infrastructure.DependencyInjection; // ✅ Se tiver um similar para Infrastructur

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// ?? Aplicação e Infraestrutura separadas
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);

// Extras
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Host.UseDefaultServiceProvider(o => {
    o.ValidateScopes = true;
    o.ValidateOnBuild = true;
});

var app = builder.Build();

// Rota de health bem simples
app.MapGet("/health", () => Results.Ok("OK")).AllowAnonymous();

// ⛑️ Middleware global de tratamento de exceções
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<ProblemDetailsExceptionMiddleware>();

// ⚙️ Swagger 
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// CORS precisa vir aqui 👇
app.UseCors();

app.UseAuthorization();
app.MapControllers();
app.Run();
