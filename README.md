
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
