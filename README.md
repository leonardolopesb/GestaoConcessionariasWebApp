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

## 🛠️ Tecnologias Utilizadas

- **.NET 8 / ASP.NET Core MVC**
- **Entity Framework Core** (SQL Server)
- **FluentValidation.AspNetCore**
- **ASP.NET Identity**
- **Swagger / Swashbuckle**
- **Bootstrap 5**
- **JavaScript Vanilla**

---

## ⚙️ Configuração do Ambiente

### 🔹 Pré-requisitos
- Visual Studio 2022 e/ou ou VS Code
- .NET 8/9 SDK
- SQL Server Express

### 🔹 Clonar o repositório
```bash
git clone https://github.com/leonardolopesb/GestaoConcessionariasWebApp.git
cd GestaoConcessionariasWebApp
```

### 🔹 Configurar o Banco de Dados
No arquivo `appsettings.json`, ajuste a connection string para o seu SQL Server:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=GestaoConcessionariasDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

### 🔹 Rodar as Migrations
```bash
dotnet ef database update
```

O comando criará o banco com todas as tabelas necessárias.

---

## ▶️ Executando o Projeto

```bash
dotnet run
```

Ao executar o código acima, o projeto estará disponível em:
- **Frontend:** [https://localhost:7020/](https://localhost:7020/)
- **Swagger:** [https://localhost:7020/swagger](https://localhost:7020/swagger)

---

## 👤 Usuário Inicial (Seeder)

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

## 📜 Licença

Licenciado pelo [MIT License](./LICENSE).

---

## 👨‍💻 Autor

[<img src='https://avatars.githubusercontent.com/u/54039202?v=4' width = 300><br><sub>Leonardo Lopes Braga</sub>](https://github.com/leonardolopesb) 
