
# 📊 FSI.PersonalFinanceApp.BackEnd

Sistema backend responsável pela gestão financeira pessoal, estruturado com base na arquitetura **Domain-Driven Design (DDD)**, utilizando boas práticas de organização, separação de responsabilidades e escalabilidade.

---

## 🧱 Arquitetura do Sistema

A aplicação segue o padrão em camadas proposto pela arquitetura DDD:

```
FSI.PersonalFinanceApp.BackEnd
├── Api                # Camada de apresentação (Web API)
├── Application        # Camada de aplicação (DTOs, serviços e interfaces de aplicação)
├── Domain             # Camada de domínio (Entidades, interfaces e regras de negócio)
├── Infrastructure     # Camada de infraestrutura (Repositorios, banco de dados, dependências externas)
├── Worker             # Serviço Worker para consumo assíncrono (ex: RabbitMQ)
```

---

## 🧩 Abstrações Utilizadas

- **Entidades**: Representações das regras de negócio na camada `Domain`.
- **DTOs**: Objetos de transporte utilizados entre camadas (`Application.Dtos`).
- **Interfaces**: Interfaces segregadas para aplicação (`IAppService`), domínio (`IRepository`, `IServices`) e infraestrutura.
- **Serviços de Domínio**: Centralizam regras que não pertencem diretamente às entidades.
- **Mappers (AutoMapper)**: Mapeamento automático entre entidades e DTOs.
- **Mensageria**: Worker para processamento assíncrono com RabbitMQ.

---

## 💉 Injeção de Dependência

A injeção de dependência é configurada em `Program.cs` e descentralizada por meio de métodos de extensão:

```csharp
builder.Services.AddApplicationServices();        // Application layer
builder.Services.AddInfrastructure(configuration); // Infrastructure layer
```

Cada camada possui sua própria classe de `DependencyInjection` que registra seus serviços, repositórios e configurações.

---

## ✍️ Padrões de Escrita

- **Clean Code**: Código limpo e legível com nomes autoexplicativos.
- **SOLID**: Princípios aplicados em toda a arquitetura.
- **Separação de Responsabilidades**: Cada camada e classe tem responsabilidade única.
- **Convention over Configuration**: Estrutura padronizada e reutilizável.
- **Tratamento Global de Erros**: Middleware global `ExceptionHandlingMiddleware` captura exceções e retorna erros padronizados.
- **Swagger**: Gerador automático de documentação para endpoints REST.

---

## 🐳 Docker

O projeto inclui `docker-compose.yml` para facilitar a orquestração de containers da aplicação, banco de dados e serviços de mensageria.

---

## 🛠️ Tecnologias e Ferramentas

- ASP.NET Core 7 Web API
- RabbitMQ (mensageria)
- AutoMapper
- Swagger (OpenAPI)
- SQL Server / PostgreSQL (configurável)
- Docker / Docker Compose

---

## 📡 Arquitetura Assíncrona e Estilos de Requisições

A API foi desenhada seguindo princípios de **microsserviços desacoplados**, oferecendo **três estilos distintos de comunicação**, cada um alinhado a um padrão arquitetural reconhecido:

### ✅ 1. Requisições Síncronas REST
**🔹 Arquitetura:** _Request/Response Pattern_

- Comunicação direta entre cliente e serviço via HTTP.
- Baixo tempo de resposta.
- Simples de implementar, porém mais acoplado e suscetível a falhas em cascata.

### 🔄 2. Requisições Assíncronas com ASP.NET (async/await)
**🔹 Arquitetura:** _Reactive Microservices Pattern_

- Opera com `Task`, `async/await` e operações não bloqueantes.
- Ideal para chamadas I/O-bound (ex: banco, API externa).
- Escalável e eficiente em servidores de alto tráfego.

### 📨 3. Requisições Assíncronas com Mensageria e Polling
**🔹 Arquitetura:** _Event-Driven Microservices (com Message Queue + Polling)_

- O cliente **envia um comando** → API publica na fila (RabbitMQ).
- A API retorna imediatamente um `correlationId` ou `messageId`.
- Um **Worker** processa a mensagem em background.
- O cliente pode fazer `GET /result/{id}` para saber o status ou obter o resultado.

#### Fluxo:
```
1. Cliente → POST /api/expense-categories/async
2. API → Retorna: { messageId: "abc-123" }
3. Worker → Consome da fila e processa
4. Cliente → GET /api/messaging/result/abc-123
```

#### Comparativo:

| Estilo de Consumo                      | Padrão Arquitetural                 | Melhor Uso                                |
|---------------------------------------|-------------------------------------|--------------------------------------------|
| Síncrono REST                         | Request/Response                    | Consultas rápidas, operações imediatas     |
| Assíncrono com async/await            | Reactive Microservices              | Escalabilidade de I/O                      |
| Assíncrono com mensageria e polling   | Event-Driven + Message Queue + Polling | Processos pesados, rastreáveis e desacoplados |

---

## 🚀 Como executar

```bash
git clone https://github.com/seu-usuario/FSI.PersonalFinanceApp.BackEnd.git
cd FSI.PersonalFinanceApp.BackEnd
docker-compose up -d
```

Acesse:
- Swagger: http://localhost:5000/swagger
- API: http://localhost:5000/api

---

## 📂 Organização de Código

| Projeto                            | Responsabilidade                                   |
|-----------------------------------|----------------------------------------------------|
| `FSI.PersonalFinanceApp.Api`      | Web API, controllers, middlewares, configuração    |
| `FSI.PersonalFinanceApp.Domain`   | Entidades, interfaces de domínio, regras de negócio|
| `FSI.PersonalFinanceApp.Application` | Serviços de aplicação, DTOs, interfaces de App     |
| `FSI.PersonalFinanceApp.Infrastructure` | Persistência, repositórios, integração externa  |
| `FSI.PersonalFinanceApp.Worker`   | Processamento assíncrono via RabbitMQ              |

---

## 🧪 Testes

*Testes unitários devem ser implementados com XUnit e Moq (não incluídos neste projeto base).*

---

## 📃 Licença

MIT - Rodrigo Luiz Madeira Furlaneti
