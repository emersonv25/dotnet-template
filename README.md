# Dotnet Template

Um template com Asp Net Core utilizando principios da arquitetura limpa. 
Criei este template para usar de referencia nos meus novos projetos e para fim de estudo do clean architecture.

## Funcionalidades

Apenas cadastro de usuários e autenticação com o Firebase, e EF Core com PostgreSql

## Tecnologias Utilizadas

- **ASP.NET Core**: Framework backend para construção da API.
- **Entity Framework Core**: ORM para comunicação com o banco de dados.
- **Firebase Authentication**: Serviço de autenticação de usuários.
- **Docker**: Para gerenciamento de contêineres.
- **MySQL / PostgreSQL / SQL Server** (exemplo de banco): Banco de dados relacional.
- **C#**: Linguagem principal do projeto.

## Configuração do Projeto

### Arquivo de configuração do Firebase
Como o projeto usa o firebase, ele depende de ter o arquivo de configuração do firebase

Baixe o arquivo de configuração no console do firebase em:

Configuração do Projeto > Contas de Serviço > SDK Admin do Firebase > Gerar nova chave privada

Após baixar o arquivo, cole em algum diretório dentro do projeto, e edite o caminho para o arquivo no appsettings.json na váriavel FirebaseCredentialPath

Exemplo:
```
  "AppSettings": {
    "FirebaseCredentialPath": "./firebase-service-account.json"
  },
```

### Comandos de Migration

```bash
dotnet ef migrations add StartMigration --project Template.Infra.Data --startup-project Template.Api
dotnet ef database update --project Template.Infra.Data --startup-project Template.Api
```

## Contribuição

- Contribuições são bem-vindas! Sinta-se à vontade para abrir um pull request ou relatar problemas.

### Contribuidores

- [emersonv25](https://github.com/emersonv25) Aprimorei o projeto e a arquitetura e transformei em um template básico apenas com entidade User padrão.
- [matheus55391](https://github.com/matheus55391) Iniciou o projeto com principios arquitetura em camada: projeto original: (https://github.com/matheus55391/partilha-api)


## Licença

Este projeto é licenciado sob a **AGPL-3.0 License**. Você é livre para usar, modificar e distribuir este software, desde que qualquer distribuição do código modificado também seja licenciada sob a AGPL-3.0.

Para mais detalhes, consulte o [texto completo da licença AGPL-3.0](https://www.gnu.org/licenses/agpl-3.0.html).
