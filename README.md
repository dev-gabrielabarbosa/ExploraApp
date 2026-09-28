# PontoTuristicoApp

Aplicação Web de cadastro e listagem de pontos turísticos do Brasil. 
Desenvolvida como teste técnico para perfil de desenvolvedor júnior.

## Tecnologias utilizadas

- **C#** e **.NET 8**
- **ASP.NET Core MVC** (Arquitetura monolítica simples)
- **Entity Framework Core**
- **SQLite** (Banco de dados leve)
- **HTML, CSS e Bootstrap 5**

## Decisões Técnicas

- **Solução Monolítica:** Conforme solicitado, o frontend e o backend rodam no mesmo projeto, facilitando o entendimento e demonstrando domínio dos fundamentos web.
- **SQLite:** Escolhido por não necessitar de instalação de um servidor SQL. O arquivo do banco (`pontosturisticos.db`) é criado localmente na pasta do projeto.
- **Entity Framework Core:** Usado como ORM pela agilidade na implementação de consultas (`LINQ`) e fácil gerência do esquema do banco de dados (Migrations).
- **Bootstrap 5:** Usado via CDN para entregar uma interface bonita, responsiva e limpa, sem gastar muito tempo escrevendo CSS do zero.
- **Auto-Migration no Startup:** Para simplificar a execução por qualquer pessoa, o projeto aplica as *migrations* automaticamente no banco de dados SQLite assim que executado, sem a necessidade de comandos adicionais do EF Tools.

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

## Como clonar o projeto

```bash
git clone <url_do_repositorio>
cd PontoTuristicoApp
```

## Banco de dados

- O projeto utiliza **SQLite**.
- A string de conexão está no `appsettings.json` apontando para o arquivo local `pontosturisticos.db`.
- O banco será criado automaticamente e as tabelas preparadas na primeira vez que você executar a aplicação. **Não é necessário rodar migrations manualmente**, o código em `Program.cs` já faz isso (`db.Database.Migrate()`).
- Caso precise rodar manualmente as migrations: `dotnet ef database update`.

## Como executar

Pelo terminal, na pasta raiz do projeto:

```bash
# 1. Restaura as dependências
dotnet restore

# 2. Executa a aplicação
dotnet run
```
Após executar, acesse no navegador: `http://localhost:5000` (ou a porta listada no terminal).

## Funcionalidades

- **Cadastro:** Inclusão de ponto turístico (nome, descrição, endereço, cidade e estado). Validações obrigatórias e limite de 100 caracteres na descrição.
- **Listagem e Paginação:** Lista os cadastros ordenados pelos mais recentes. Limite de 5 itens por página para demonstrar a paginação facilmente.
- **Busca/Filtro:** Campo de busca que filtra os registros por Nome, Descrição e Localização integrando perfeitamente com a paginação.
- **Visualização de Detalhes:** Exibição completa de um ponto turístico específico através do botão "Detalhes".
- **Navegação:** Menu superior fixo facilitando a transição entre Lista e Cadastro.

## Critérios de Aceite Atendidos
- [x] Aplicação Web em C# e ASP.NET Core MVC (Monolítica)
- [x] SQLite configurado (Entity Framework)
- [x] Cadastro de pontos turísticos (Descrição máx. 100 caracteres e Estado via Dropdown)
- [x] Listagem com ordenação decrescente por data e paginação
- [x] Busca (nome, descrição e localização)
- [x] Validações claras (Backend e Frontend via Unobtrusive JS)
- [x] Interface limpa e responsiva (Bootstrap)
- [x] Estrutura simples para nível Júnior
- [x] Versionamento Git (Mínimo de 2 commits)
