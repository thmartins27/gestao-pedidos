# Gestão de Pedidos

Sistema de gestão de pedidos com API REST em .NET 10 e frontend em Angular 21.

---

## Índice

- [Como rodar a aplicação](#como-rodar-a-aplicação)
  - [Pré-requisitos](#pré-requisitos)
  - [Opção 1 — Docker (recomendado)](#opção-1--docker-recomendado)
  - [Opção 2 — Execução local](#opção-2--execução-local)
  - [Endereços da aplicação](#endereços-da-aplicação)
  - [Rodando os testes](#rodando-os-testes)
- [Tecnologias utilizadas](#tecnologias-utilizadas)
  - [Backend](#backend)
  - [Frontend](#frontend)
  - [Infraestrutura](#infraestrutura)
- [Estrutura do projeto](#estrutura-do-projeto)
- [Funcionalidades](#funcionalidades)
- [Decisões de arquitetura](#decisões-de-arquitetura)
- [O que faria diferente com mais tempo](#o-que-faria-diferente-com-mais-tempo)
- [Evolução: notificações por e-mail](#evolução-notificações-por-e-mail)

---

## Como rodar a aplicação

### Pré-requisitos

| Cenário | Necessário |
| --- | --- |
| Docker | Docker Desktop / Docker Engine com Compose v2 |
| Execução local | .NET SDK 10, Node.js 24+, pnpm 11 e uma instância do PostgreSQL 17 |

### Opção 1 — Docker (recomendado)

Sobe banco, backend e frontend com um único comando, a partir da raiz do projeto:

```bash
docker compose up -d --build
```

O backend aplica as migrations e popula o seed automaticamente na primeira subida, aguardando o healthcheck do PostgreSQL.

Para encerrar:

```bash
docker compose down          # mantém os dados do banco
docker compose down -v       # remove também o volume do banco
```

### Opção 2 — Execução local

**1. Banco de dados** — subir apenas o PostgreSQL via Docker:

```bash
docker compose up -d postgres
```

**2. Backend** — na raiz do projeto:

```bash
dotnet run --project backend/GestaoPedidos.Api
```

O `dotnet run` já restaura os pacotes NuGet e, no startup, aplica as migrations e o seed do banco — não é necessário rodar nenhum comando de migration manualmente.

<details>
<summary>Dependência nativa no Linux</summary>

O Npgsql precisa da biblioteca de Kerberos do sistema. Na maioria das distribuições desktop ela já vem instalada; caso o backend falhe ao conectar no banco com erro relacionado a `libgssapi`, instale:

```bash
# Debian / Ubuntu
sudo apt-get install -y libgssapi-krb5-2

# Fedora / RHEL
sudo dnf install -y krb5-libs
```

No Windows e no macOS não é necessário nada além do .NET SDK.

</details>

**3. Frontend** — na pasta `frontend/`:

```bash
pnpm install
pnpm start
```

### Endereços da aplicação

| Serviço | Docker | Local |
| --- | --- | --- |
| Frontend | http://localhost:4200 | http://localhost:4200 |
| API | http://localhost:5196 | http://localhost:5196 |
| Swagger | http://localhost:5196/swagger | http://localhost:5196/swagger |
| PostgreSQL | localhost:5432 | localhost:5432 |

### Rodando os testes

**Backend** (xUnit) — na pasta `backend/`:

```bash
dotnet test
```

> Os testes de integração usam Testcontainers e exigem o Docker em execução.

**Frontend** (Vitest) — na pasta `frontend/`:

```bash
pnpm test
```

---

## Tecnologias utilizadas

### Backend

| Tecnologia | Versão | Uso |
| --- | --- | --- |
| .NET | 10 | Plataforma base |
| ASP.NET Core | 10 | API REST com Controllers |
| Entity Framework Core | 10.0.10 | ORM e migrations |
| Npgsql.EntityFrameworkCore.PostgreSQL | 10.0.3 | Provider do PostgreSQL |
| FluentValidation.AspNetCore | 11.3.1 | Validação dos DTOs de entrada |
| Serilog.AspNetCore | 10.0.0 | Logs estruturados (console e arquivo) |
| Microsoft.AspNetCore.OpenApi | 10.0.9 | Geração do documento OpenAPI |
| Swashbuckle.AspNetCore.SwaggerUI | 10.2.3 | Interface do Swagger |
| xUnit v3 | 3.2.2 | Testes unitários e de integração |
| Moq | 4.20.72 | Mocks nos testes de serviço |
| Testcontainers.PostgreSql | 4.13.0 | PostgreSQL efêmero nos testes de integração |

### Frontend

| Tecnologia | Versão | Uso |
| --- | --- | --- |
| Angular | 21.2 | Framework (standalone components e signals) |
| TypeScript | 5.9 | Linguagem |
| Angular Material + CDK | 21.2 | Biblioteca de componentes de UI |
| Reactive Forms (`@angular/forms`) | 21.2 | Formulários e validações |
| RxJS | 7.8 | Programação reativa e chamadas HTTP |
| Vitest | 4.0 | Testes unitários e de componente |
| pnpm | 11.7 | Gerenciador de pacotes |

### Infraestrutura

| Tecnologia | Versão | Uso |
| --- | --- | --- |
| PostgreSQL | 17 (alpine) | Banco de dados |
| Docker Compose | v2 | Orquestração dos containers |
| Nginx | alpine | Servidor do build do frontend e proxy para a API |

---

## Estrutura do projeto

```
.
├── backend/            API .NET e projeto de testes
├── frontend/           Aplicação Angular
├── docker-compose.yml  Orquestração do postgres, backend e frontend
└── global.json         Versão do SDK .NET usada no projeto
```

O backend segue a divisão em três camadas, com as regras de negócio nos serviços e nas próprias entidades:

```
backend/
├── GestaoPedidos.Api/
│   ├── Controllers/     Endpoints HTTP
│   ├── Services/        Regras de negócio
│   ├── Repositories/    Acesso a dados com EF Core
│   ├── Models/          Entidades de domínio
│   ├── DTOs/            Contratos de entrada e saída
│   ├── Validators/      Validações com FluentValidation
│   ├── Data/            DbContext, mapeamentos e seed
│   ├── Middlewares/     Tratamento global de erros
│   ├── Exceptions/      Exceções de domínio
│   ├── Extensions/      Registro das dependências
│   ├── OpenApi/         Ajustes do schema do Swagger
│   └── Migrations/      Migrations do EF Core
└── GestaoPedidos.Tests/
    ├── Models/          Testes das regras de domínio
    ├── Services/        Testes de serviço com mocks
    └── Integration/     Testes da API com Testcontainers
```

No frontend, `core` concentra o que é transversal, `features` as telas e `shared` os componentes reaproveitados:

```
frontend/src/
├── app/
│   ├── core/
│   │   ├── services/       Chamadas à API
│   │   └── interceptors/   Log e tratamento de erros
│   ├── features/           Telas da aplicação
│   ├── shared/components/  Componentes reutilizáveis
│   └── models/             Interfaces e tipos
└── environments/           URL da API por ambiente
```

---

## Funcionalidades

Regras de negócio aplicadas na criação e na atualização de pedidos:

- O pedido não é criado se algum item não tiver estoque suficiente
- O valor total é calculado a partir dos itens do pedido
- O estoque dos produtos é decrementado na criação
- O estoque é devolvido quando o pedido é cancelado

As transições de status são validadas no domínio:

```
Pendente  → Pago ou Cancelado
Pago      → Cancelado
Cancelado → estado final
```

Telas da aplicação:

| Tela | O que faz |
| --- | --- |
| Pedidos | Lista com filtro por status e paginação |
| Novo pedido | Seleção de cliente, inclusão de produtos e total atualizado a cada item |
| Produtos | Listagem e cadastro |
| Dashboard | Total de pedidos, valor total e quantidade por status |

A mudança de status é feita por um modal na listagem, que oferece apenas as transições válidas para o pedido.

As rotas, os DTOs e os schemas da API estão documentados no Swagger, disponível em http://localhost:5196/swagger com a aplicação em execução.

---

## Decisões de arquitetura

**Tratamento de erros centralizado.** No backend as exceções de domínio são traduzidas para HTTP em um middleware único, o que evita `try/catch` espalhado pelos controllers. No frontend um interceptor extrai a mensagem da resposta de erro e a repassa pronta para as telas, que só precisam exibi-la.

**PostgreSQL real nos testes de integração.** Os testes sobem um banco efêmero com Testcontainers em vez de usar o provider InMemory. Exige Docker e deixa a suíte mais lenta, em troca de cobrir constraints e comportamento do provider que o InMemory não reproduz.

**Migrations e seed aplicados no startup.** A aplicação sobe pronta para uso, sem passo manual. Em produção esse tipo de execução costuma ficar a cargo do pipeline de deploy, separada da inicialização.

---

## O que faria diferente com mais tempo

No backend, aplicaria Clean Architecture separando domínio, aplicação e infraestrutura em projetos distintos, e não apenas em pastas, com CQRS dividindo comandos de escrita e consultas de leitura. Isso daria o lugar natural para tratar a concorrência no estoque: hoje a verificação e a baixa acontecem entre a leitura e o commit, sem bloqueio, então dois pedidos simultâneos do mesmo produto podem passar os dois pela validação. Somaria a isso autenticação com JWT, protegendo os endpoints por perfil de usuário.

No frontend, incluiria a tela de cadastro de clientes, que hoje não existe — a API já tem o CRUD completo, mas a interface só consome a listagem para o select de pedidos — e uma tela de login consumindo essa autenticação, com guard de rota e o token enviado pelo interceptor. Estenderia o ajuste de responsividade para as demais telas, já que dashboard e novo pedido foram pensados para telas estreitas e as outras não.

A cobertura de testes também poderia estar mais completa. Hoje existem testes das regras de domínio, dos serviços com mock e de integração no backend, e no frontend um service e dois componentes cobertos, faltando as telas de listagem e o restante dos services.

Por fim, montaria um pipeline de CI/CD rodando build e testes a cada push e publicando a aplicação automaticamente a partir da branch principal.

---

## Evolução: notificações por e-mail

> Se fosse adicionar um sistema de notificações por e-mail ao mudar o status do pedido, onde e como você integraria isso no código atual?

Faria o envio em um serviço separado, acionado somente depois que a alteração de status estivesse concluída e gravada. Em vez de enviar o e-mail durante a requisição, esse serviço publicaria a mensagem em uma fila e a resposta voltaria na hora para o frontend, sem deixar o usuário esperando o provedor de e-mail responder. Um worker consome essa fila em segundo plano e cuida do envio, podendo repetir a tentativa em caso de falha sem impacto no pedido, que já está persistido. Usaria Redis como fila, mesmo padrão que costumo aplicar em Node.