# 🔗 URL Shortener Service

A minimal **ASP.NET Core Web API** for shortening URLs, built with **Clean Architecture** principles. Supports creating, retrieving, redirecting, and deleting short URL mappings — with built-in input validation and global exception handling.

---

## 🏗️ Architecture

This project follows **Clean Architecture (Domain-Driven Design)**, separating concerns across distinct layers:

```
URL-Shortener-Service/
├── Controllers/          → API endpoints (HTTP layer)
├── Models/               → Request & Response DTOs
├── Attributes/           → Custom validation attributes
├── Middleware/           → Global exception handling
├── Extensions/           → Service registration (AddApplication)
├── MyApi.Domain/         → Entities, interfaces (core business logic)
├── MyApi.Infrastructure/ → EF Core, DbContext, repositories
└── Program.cs            → App entry point & middleware pipeline
```

**Why Clean Architecture?**
Keeping the domain layer free from infrastructure concerns (like EF Core or PostgreSQL) makes the business logic independently testable and easy to extend — for example, swapping PostgreSQL for another database requires changes only in the Infrastructure layer.

---

## 🛠️ Tech Stack

| Layer          | Technology                        |
|----------------|-----------------------------------|
| Framework      | ASP.NET Core Web API (.NET 8)     |
| ORM            | Entity Framework Core             |
| Database       | PostgreSQL (hosted on Neon)       |
| Documentation  | Swagger / OpenAPI                 |
| Architecture   | Clean Architecture + DDD          |

---


## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL database (local or cloud — [Neon](https://neon.tech) free tier works great)

### 1. Clone the repository

```bash
git clone https://github.com/Balanjaneyasharma/URL-Shortener-Service.git
cd URL-Shortener-Service
```

### 2. Configure the database connection

Create `appsettings.Development.json` in the project root (this file is gitignored — never commit real credentials):

```json
{
  "ConnectionStrings": {
    "DBConnection": "Host=your-host;Database=your-db;Username=your-user;Password=your-password"
  }
}
```

Or use .NET User Secrets (recommended):

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DBConnection" "your-connection-string"
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

### 5. Explore the API

Open Swagger UI at:
```
https://localhost:5030/swagger
```

---

## 🔒 Security Notes

- Database credentials are **never committed** to this repository
- Connection strings are managed via `appsettings.Development.json` (gitignored) or environment variables
- For production deployments, set credentials as environment variables:

```
ConnectionStrings__DBConnection=your-production-connection-string
```

---

## ✨ Features

- **Short code generation** — unique alphanumeric codes for each URL
- **Redirect support** — clean HTTP redirect to the original URL
- **Input validation** — invalid URLs are rejected with meaningful error responses
- **Global exception handling** — consistent error responses across all endpoints via custom middleware
- **Swagger UI** — interactive API documentation in development mode
- **Layered architecture** — Domain and Infrastructure layers properly separated

---

## 🚀 Planned Improvements

- [ ] Custom alias support — let users choose their own short code
- [ ] Click analytics — track how many times each link was visited
- [ ] Link expiry — auto-expire URLs after a configurable duration
- [ ] Rate limiting — prevent API abuse
- [ ] Frontend — UI to create and manage short URLs
- [ ] Deploy to Railway / Render with live Swagger link

---

## 🧠 What I Learned Building This

Key things this project taught me:

- How ASP.NET Core middleware pipeline works (and why order matters)
- Structuring a Web API with Clean Architecture and why it's worth the extra setup
- EF Core migrations and working with a real cloud PostgreSQL database
- How HTTP redirects work (301 vs 302) and when to use each
- Managing secrets safely — never committing credentials to git
