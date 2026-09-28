# ExploraApp

Aplicação Web de cadastro e listagem de pontos turísticos do Brasil.

## Tecnologias utilizadas

- **C#** e **.NET 8**
- **ASP.NET Core MVC** (Arquitetura monolítica simples)
- **Entity Framework Core**
- **SQLite** (Banco de dados leve)
- **HTML, CSS, JavaScript e Bootstrap 5**
- **API de Localidades do IBGE** (Integração HTTP via HttpClient)

## Decisões Técnicas

- **Solução Monolítica:** O frontend e o backend rodam no mesmo projeto, facilitando o entendimento e a execução.
- **Integração com IBGE:** A comunicação com a API do IBGE é centralizada na camada de serviço (`IbgeService`) utilizando `HttpClient` com injeção de dependência e cache em memória (`IMemoryCache`), prevenindo chamadas desnecessárias e garantindo performance e resiliência.
- **SQLite:** Escolhido por não necessitar de instalação de um servidor SQL. O arquivo do banco (`pontosturisticos.db`) é criado localmente na pasta do projeto.
- **Entity Framework Core:** Usado como ORM pela agilidade na implementação de consultas (`LINQ`) e fácil gerência do esquema do banco de dados.
- **Bootstrap 5:** Usado via CDN para entregar uma interface bonita, responsiva e limpa.

## Integração com a API do IBGE

A aplicação consome a API oficial de Localidades do IBGE para carregar dinamicamente os Estados e os Municípios correspondentes:

### Endpoints Utilizados:
1. **Estados:** `https://servicodados.ibge.gov.br/api/v1/localidades/estados?orderBy=nome`
2. **Municípios por Estado:** `https://servicodados.ibge.gov.br/api/v1/localidades/estados/{UF_ID}/municipios?orderBy=nome` (onde `{UF_ID}` é o ID numérico do Estado no IBGE, ex: `35` para SP).

### Fluxo da Integração:
`Estado selecionado no Dropdown` → `Captura do ID numérico da UF (ex: 35)` → `Requisição AJAX para Action MVC (/PontosTuristicos/MunicipiosPorEstado/35)` → `Backend consulta API do IBGE` → `Retorno em JSON` → `Dropdown de Cidade atualizado e habilitado via JavaScript`

## Pré-requisitos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

## Como clonar o projeto

```bash
git clone <url_do_repositorio>
cd exploraApp
```

## Banco de dados

- O projeto utiliza **SQLite**.
- A string de conexão está no `appsettings.json` apontando para o arquivo local `pontosturisticos.db`.
- O banco será criado automaticamente e as tabelas preparadas na primeira vez que você executar a aplicação (`EnsureCreated()`).

## Como executar

Pelo terminal, na pasta raiz do projeto:

```bash
# 1. Restaura as dependências
dotnet restore

# 2. Compila e executa a aplicação
dotnet run
```
Após executar, acesse no navegador: `http://localhost:5000` (ou o endereço retornado no terminal).

## Funcionalidades

- **Cadastro Dinâmico:** Inclusão de ponto turístico com seleção dinâmica de Estado e Cidade carregados via API do IBGE.
- **Listagem e Paginação:** Lista os cadastros ordenados pelos mais recentes com paginação de 5 itens por página.
- **Busca/Filtro:** Campo de busca que filtra por Nome, Cidade, Estado/UF, Descrição e Localização.
- **Visualização de Detalhes:** Exibição completa das informações do ponto turístico.
- **Tratamento de Erros e Resiliência:** Indicadores de carregamento, desabilitação temporária de campos e mensagens amigáveis em caso de falha da API externa.

## Requisitos do Teste Cumpridos

### 📌 Requisitos Obrigatórios
- [x] **Aplicação de Cadastro e Listagem de Pontos Turísticos:** Cadastro contendo Nome, Descrição (até 100 caracteres), Localização, Cidade e Estado.
- [x] **Página Inicial (Listagem, Ordenação e Paginação):** Exibição paginada dos pontos turísticos com Nome e Localização, ordenados de forma decrescente pela Data de Inclusão.
- [x] **Busca e Filtro:** Campo de busca funcional (case-insensitive) que filtra por Nome, Descrição, Localização, Cidade e Estado/UF.
- [x] **Visualização de Detalhes:** Exibição completa das informações ao selecionar um ponto turístico (Nome, Descrição, Localização, Cidade e Estado/UF).
- [x] **Formulário de Cadastro com Seleção Dinâmica:** Dropdown de Estados e busca assíncrona das Cidades correspondentes via webservice público da API do IBGE.
- [x] **Menu de Navegação:** Navegação no cabeçalho da aplicação para alternar entre a listagem inicial e o formulário de cadastro.
- [x] **Modelo de Solução Monolítica (Perfil Júnior):** Aplicação Web monolítica simples desenvolvida em C# e ASP.NET Core MVC.
- [x] **Banco de Dados Relacional:** Utilização do SQLite configurado via Entity Framework Core com criação automática de tabelas (`EnsureCreated()`).
- [x] **Controle de Versão com Git:** Histórico de commits mantido via Git.
- [x] **Documentação em Markdown (README.md):** Arquivo README na raiz com instruções completas para clonar, compilar, configurar o banco e executar o projeto.

### 🌟 Requisitos Opcionais (Diferenciais Cumpridos)
- [x] **Conceitos de POO e Boas Práticas (Clean Code):** Código estruturado, limpo, fortemente tipado, com injeção de dependências e convenções de nomenclatura do .NET.
- [x] **Separação de Responsabilidades:** Organização clara das camadas de acesso a dados (`Data`), modelos/DTOs/ViewModels (`Models`), regra de negócio e integração HTTP (`Services`), controle de requisições (`Controllers`) e interface gráfica (`Views`).
