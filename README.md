# .NET API with Entity Framework (SQL Server) – Focused on Testing

This is a simple RESTful API built with ASP.NET Core and Entity Framework, designed primarily to demonstrate unit testing and integration testing workflows using xUnit and Moq. The project supports SQL Server as the main database engine, making it suitable for production and enterprise scenarios.

## 📦 Features

- RESTful API structure (CRUD)
- Entity Framework Core integration (SQL Server/SQLite/InMemory)
- Unit testing with xUnit & Moq
- Integration testing with WebApplicationFactory
- Environment configuration via appsettings.json

## 🏗️ Project Structure

```
dotnet-api-testing/
│
├── Controllers/
│   └── TodoController.cs
├── Models/
│   └── Todo.cs
├── Data/
│   └── AppDbContext.cs
├── Services/
│   ├── ITodoService.cs
│   └── TodoService.cs
├── Properties/
│   └── launchSettings.json
├── Tests/
│   ├── Unit/
│   │   ├── TodoControllerTest.cs
│   │   └── TodoServiceTest.cs
│   └── Integration/
│       └── TodoIntegrationTest.cs
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
├── dotnet-api-testing.csproj
└── README.md
```

## 🧪 Testing Goals

- **Unit Testing**: Isolate individual modules (controllers/services) using Moq.
- **Integration Testing**: Ensure API endpoints work as expected, including DB interaction, using in-memory database.

## 🛠️ Installation

```bash
git clone https://github.com/your-username/dotnet-api-testing.git
cd dotnet-api-testing
dotnet restore
```

## 🧾 Configuration

Database and environment configuration are located in `appsettings.json` and `appsettings.Development.json`.

Example configuration for SQL Server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=todo_db;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

## 🗃️ Run Migrations (if any)

If you use EF Core and SQL Server, run:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Example table for SQL Server:

```sql
CREATE TABLE Todos (
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Title NVARCHAR(100) NOT NULL,
  IsCompleted BIT NOT NULL DEFAULT 0
);
```

## 🚀 Running the App

```bash
dotnet run
```

## 🧪 Running Tests

- Run unit tests:

```bash
dotnet test --filter Category=Unit
```

- Run integration tests:

```bash
dotnet test --filter Category=Integration
```

- Or all tests:

```bash
dotnet test
```

## 🧪 Testing Commands

### Unit Test
- Run unit tests for controller:
  ```bash
  dotnet test --filter FullyQualifiedName~TodoControllerTest
  ```
- Run unit tests for service:
  ```bash
  dotnet test --filter FullyQualifiedName~TodoServiceTest
  ```

### Integration Test
- Run integration tests for API endpoints:
  ```bash
  dotnet test --filter FullyQualifiedName~TodoIntegrationTest
  ```

- To run all tests:
  ```bash
  dotnet test
  ```

## 📬 API Endpoints

| Method | Endpoint          | Description        |
|--------|-------------------|--------------------|
| GET    | /api/todos        | Get all todos      |
| GET    | /api/todos/{id}   | Get todo by ID     |
| POST   | /api/todos        | Create new todo    |
| PUT    | /api/todos/{id}   | Update todo        |
| DELETE | /api/todos/{id}   | Delete todo        |

## 🧪 Postman Testing

You can test the API using [Postman](https://postman.com) by importing the following endpoints and sending requests to:

```
http://localhost:5000/api/todos
```

Make sure the database is running and the correct connection string is configured.

---

## ✅ Contribution

Feel free to fork, open issues, or PRs to improve this testing boilerplate.

## 📝 License

MIT License
