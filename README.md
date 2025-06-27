
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

## ✅ Camadas de Testes Automatizados

A solução utiliza uma abordagem robusta e em camadas para garantir a qualidade, confiabilidade e manutenibilidade do sistema, cobrindo desde testes unitários até testes ponta-a-ponta. Cada camada tem uma função específica no ciclo de testes:

### 1. 🧪 Unit Tests (`FSI.PersonalFinanceApp.UnitTests`)
**Objetivo**: Validar o comportamento de unidades isoladas de código, como métodos e classes individuais, sem dependências externas.

**Escopo de Teste**:
- Foca em testar regras de negócio puras da camada `Domain`.
- Serviços da `Application` com mocks simulando dependências.
- Testes rápidos, confiáveis e executados inteiramente em memória.

**Exemplos**:
- Validação de entidades (ex: campos obrigatórios, formatos).
- Lógica de domínio (ex: cálculo de saldo, aplicação de regras financeiras).
- Comportamento de serviços com entradas e saídas previsíveis.

---

### 2. 🔗 Integration Tests (`FSI.PersonalFinanceApp.IntegrationTests`)
**Objetivo**: Verificar a integração entre módulos do sistema e dependências reais, como banco de dados e repositórios.

**Escopo de Teste**:
- Testa a aplicação de forma mais ampla, envolvendo banco de dados, repositórios e serviços reais.
- Valida se os contratos entre camadas (Application → Infrastructure) estão funcionando corretamente.

**Exemplos**:
- Execução de queries e procedures reais via repositórios.
- Persistência e leitura de dados consistentes no banco de dados.
- Comportamento de serviços ao acessar dados reais.

---

### 3. 📦 Component Tests (`FSI.PersonalFinanceApp.ComponentTests`)
**Objetivo**: Validar componentes específicos da aplicação com maior escopo do que os testes unitários, mas ainda com controle sobre dependências.

**Escopo de Teste**:
- Testa controladores, serviços e componentes integrados de forma isolada.
- Usa mocks ou fakes para simular o comportamento de dependências externas.

**Exemplos**:
- Testar o comportamento de um endpoint da API sem acionar banco de dados real.
- Verificar a transformação de um DTO para uma entidade.
- Execução de casos de uso completos com dependências simuladas.

---

### 4. 🤝 Contract Tests (`FSI.PersonalFinanceApp.ContractTests`)
**Objetivo**: Garantir que os contratos de comunicação entre sistemas estejam corretos, especialmente em integrações baseadas em mensagens ou APIs REST.

**Escopo de Teste**:
- Utiliza **Pact** para definir e validar contratos entre produtor e consumidor de APIs.
- Garante compatibilidade entre serviços internos e sistemas externos.

**Exemplos**:
- Verificação de que os endpoints da API respeitam o contrato definido por um cliente.
- Testes de serialização e estrutura de payloads de mensagens.

---

### 5. 🌐 End-to-End Tests (`FSI.PersonalFinanceApp.EndToEndTests`)
**Objetivo**: Testar o sistema como um todo, simulando o comportamento real do usuário e o fluxo completo da aplicação.

**Escopo de Teste**:
- Executa cenários reais de ponta a ponta: desde a chamada à API até a persistência no banco ou envio de mensagens.
- Valida a integração completa entre módulos, serviços, banco de dados e mensageria.

**Exemplos**:
- Cadastro de uma despesa e posterior listagem pela API.
- Fluxo completo de publicação e consumo de mensagens RabbitMQ.
- Teste de persistência e recuperação com dados reais.

---

Essas camadas fornecem uma cobertura abrangente do sistema, permitindo identificar erros rapidamente em diferentes estágios do desenvolvimento.

---

## 🧱 Arquitetura e Estratégia de Testes

Este projeto está claramente estruturado com base na arquitetura **Domain-Driven Design (DDD)**, evidenciado por sua organização em camadas bem definidas:

- `Api`: Responsável pela interface com o mundo externo (controladores HTTP).
- `Application`: Contém os serviços de aplicação, interfaces, DTOs e orquestração da lógica.
- `Domain`: Núcleo da lógica de negócio, com entidades, agregados e contratos de repositório.
- `Infrastructure`: Implementações técnicas de repositórios, conexões com banco de dados, etc.
- `Worker`: Serviços desacoplados para processamento assíncrono de mensagens (ex: via RabbitMQ).

Essa separação de responsabilidades garante maior coesão, manutenibilidade e escalabilidade ao sistema, alinhando-se às boas práticas de engenharia de software moderna.

### 🚦 Estratégia de Testes e Relação com TDD

Além da arquitetura DDD, o projeto também demonstra uma estrutura de testes bastante abrangente, com múltiplas camadas de verificação da aplicação, incluindo testes unitários, de integração, de contrato e ponta-a-ponta.

Essa estrutura de testes permite — e até incentiva — a adoção da prática de **TDD (Test-Driven Development)**, pois proporciona os recursos necessários para escrever testes antes do código de produção, seguindo o ciclo clássico Red ➝ Green ➝ Refactor.

> ⚠️ **Importante**: Apesar da infraestrutura de testes bem elaborada, não há evidências diretas de que o projeto siga **TDD de forma sistemática**. Ou seja, não se pode afirmar que os testes são sempre escritos **antes** da implementação, como requer essa metodologia.

Se desejado, o projeto pode facilmente ser evoluído para adotar o TDD como prática formal, visto que já possui os fundamentos técnicos necessários.


## 🔍 Diferença entre DDD e TDD

Embora os termos **DDD (Domain-Driven Design)** e **TDD (Test-Driven Development)** sejam frequentemente utilizados em projetos modernos, é importante entender que eles têm propósitos distintos:

| Aspecto                     | DDD (Domain-Driven Design)                                            | TDD (Test-Driven Development)                                 |
|----------------------------|------------------------------------------------------------------------|---------------------------------------------------------------|
| **Tipo**                   | Arquitetura / Estratégia de Modelagem                                 | Prática de Desenvolvimento                                    |
| **Foco Principal**         | Organizar o sistema em torno do domínio do negócio                    | Garantir qualidade e design limpo via testes                  |
| **Objetivo**               | Criar um modelo de domínio rico e coeso, com separação de responsabilidades | Escrever testes antes da implementação para guiar o design   |
| **Aplicação**              | Reflete regras de negócio, entidades, agregados, serviços, repositórios | Testes unitários, de integração e refatoração orientada       |
| **Benefícios**             | Manutenibilidade, escalabilidade, entendimento do negócio              | Código mais confiável, menos bugs, melhor design              |
| **Complementaridade**      | Pode ser combinado com TDD para maior eficácia                         | Pode ser aplicado em projetos com ou sem DDD                  |

### ✅ Em resumo:
- **DDD** ajuda a estruturar o **"o que"** e **"como"** o sistema é organizado com base no negócio.
- **TDD** ajuda a definir **"quando"** e **"como"** o código deve ser implementado com segurança e qualidade.

O projeto atual aplica **DDD como estrutura arquitetural principal** e possui a base ideal para incorporar **TDD como prática de desenvolvimento.**
---

## 📃 Licença

MIT - Rodrigo Luiz Madeira Furlaneti

---

