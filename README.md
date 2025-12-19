# Sistema de Questionários Online - .NET 8

Sistema completo de pesquisas online desenvolvido com arquitetura distribuída usando .NET 8, totalmente containerizado para facilitar o deploy e desenvolvimento.

## Características

- **Frontend:** Blazor Server (Containerizado)
- **Backend:** ASP.NET Core Web API com Swagger (Containerizado)
- **Banco de Dados:** SQL Server 2022 (Docker)
- **Cache:** Redis para alta performance (Docker)
- **Mensageria:** RabbitMQ para processamento assíncrono (Docker)
- **Worker Service:** Processamento de resultados em background (Containerizado)
- **Auto-Migration:** Banco de dados e tabelas criados automaticamente ao iniciar

## Como Iniciar (Quick Start)

### Pré-requisitos

1. Docker Desktop ou Docker Engine instalado
2. Git para clonar o repositório

### Passo Único: Clonar e Rodar

Abra o terminal na pasta onde deseja salvar o projeto e execute:

#### 1. Clonar o repositório

```bash
git clone https://github.com/fabio-stoppa/SistemaQuestionario.git
cd SistemaQuestionarios
```

#### 2. Iniciar todo o ecossistema (Build e Start)

```bash
docker-compose up -d --build
```

**Nota:** Na primeira execução, a API aguardará alguns segundos para o SQL Server inicializar e então criará o banco de dados e as tabelas automaticamente.

## Acessando o Sistema

Após o comando terminar, você pode acessar os serviços através das seguintes URLs:

- **Frontend Web (Blazor):** http://localhost:7002
- **API Swagger:** http://localhost:7001/swagger
- **RabbitMQ Management:** http://localhost:15672 (Login: `admin` / Senha: `admin123`)

## Estrutura do Projeto

- **Domain:** Entidades de domínio e interfaces principais
- **Application:** DTOs e lógica de serviços de aplicação
- **Infrastructure:** Implementações de EF Core, Repositórios, Redis e RabbitMQ
- **Api:** Endpoints REST e orquestração de comandos
- **Web:** Interface do usuário em Blazor Server
- **Worker:** Serviço em background que processa respostas e atualiza estatísticas

## Dicas de Desenvolvimento

### Monitorando os Logs

```bash
docker-compose logs -f api
```

### Reiniciar do Zero (Limpar tudo)

```bash
docker-compose down -v
docker-compose up -d --build
```
