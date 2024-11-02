# Infinite.Core.Postgres

**Infinite.Core.Postgres** é uma biblioteca .NET projetada para facilitar a integração com bancos de dados PostgreSQL. Ela oferece uma série de funcionalidades que simplificam a configuração, o gerenciamento de conexões e a execução de operações comuns no banco de dados.

## Recursos

- **Configuração Simplificada**: Facilita a configuração de string de conexão e outros parâmetros essenciais.
- **Suporte a Múltiplos Esquemas**: Possibilidade de trabalhar com tabelas distribuídas em diferentes esquemas.
- **Compatibilidade com Entity Framework**: Integrável com o Entity Framework para manipulação de dados de forma rápida e segura.
- **Integração com o Consul**: Carrega automaticamente configurações de banco de dados armazenadas no Consul.

## Pré-requisitos

- **.NET 8.0 ou superior**
- **PostgreSQL 12** ou superior para compatibilidade total

## Instalação

Para instalar o pacote, você pode utilizar o seguinte comando NuGet:

```bash
dotnet add package Infinite.Core.Postgres --version 1.0.x-preview
