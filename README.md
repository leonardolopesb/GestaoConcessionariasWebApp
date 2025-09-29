# Gestão de Concessionárias

Sistema completo para **gestão de concessionárias**, desenvolvido em **HTML, CSS e JS, ASP.NET Core MVC** com **Entity Framework Core** e **SQL Server**.

Permite gerenciar **usuários, fabricantes, veículos, concessionárias, clientes e vendas**, com autenticação e controle de acesso por nível (**Admin, Gerente e Vendedor**).

---

## Funcionalidades

- Autenticação com **ASP.NET Identity**
- Controle de acesso por **roles** (Admin, Gerente, Vendedor)
- CRUD de:
  - Usuários
  - Fabricantes
  - Concessionárias
  - Veículos
  - Clientes
  - Vendas
- **FluentValidation** para validações de DTOs
- **Swagger** para documentação e teste das APIs
- **Bootstrap 5** no front-end
- Integração com **ViaCEP** (validação de endereço por CEP)

---

## Tecnologias Utilizadas

- **ASP.NET MVC**
- **HTML, CSS e JavaScript**
- **SQL Server**
- **Entity Framework Core **
- **FluentValidation**
- **ASP.NET Identity**
- **Swagger**
- **Bootstrap**

---

## Configurando o Ambiente

### Pré-requisitos
- Visual Studio 2022 e/ou VS Code
- .NET 8 ou 9 SDK
- SQL Server Express

### Clone o repositório
```bash
git clone https://github.com/leonardolopesb/GestaoConcessionariasWebApp.git
cd GestaoConcessionariasWebApp
```

### Configure o Banco de Dados
No arquivo `appsettings.json`, ajuste a connection string para o seu SQL Server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GestaoConcessionariasDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### Rode a Migration do Projeto
```bash
dotnet ef database update
```

Então, o comando criará o banco com todas as tabelas necessárias.

---

## Execute o Projeto

```bash
dotnet run
```

Ao executar o código acima, o projeto estará disponível em:
- **Frontend:** [http://localhost:5298/](http://localhost:5298/)
- **Swagger:** [http://localhost:5298/swagger](http://localhost:5298/swagger)

---

## Usuário Inicial (Seeder)

O projeto possui um **IdentitySeeder**. Para criar um administrador, capaz de cadastrar novos usuários, escreva no **Terminal**:

`dotnet user-secrets init`

`dotnet user-secrets set "ADMIN_EMAIL" "admin@local.com"`

`dotnet user-secrets set "ADMIN_USERNAME" "admin"`

`dotnet user-secrets set "ADMIN_PASSWORD" "Admin@123"`

Então você terá a conta inicial para login:

- **Usuário:** `admin`  
- **Senha:** `Admin@123`  
- **Role:** `Admin`

---

## Licença

Licenciado pelo [MIT License](./LICENSE).

---

## Autor

[<img src='https://avatars.githubusercontent.com/u/54039202?v=4' width = 300><br><sub>Leonardo Lopes Braga</sub>](https://github.com/leonardolopesb) 
