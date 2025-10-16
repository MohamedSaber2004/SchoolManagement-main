# 🎓 School Management System

A comprehensive school management REST API built with **ASP.NET Core 8.0** following **Clean Architecture** principles and **CQRS** pattern with **MediatR**.

## 📋 Table of Contents

- [Overview](#-overview)
- [Features](#-features)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Database Setup](#-database-setup)
- [API Documentation](#-api-documentation)
- [Project Structure](#-project-structure)
- [Configuration](#-configuration)
- [Development](#-development)
- [Troubleshooting](#-troubleshooting)

---

## 🎯 Overview

The School Management System is a robust backend API that manages students, classes, courses, and enrollments. It features JWT authentication, role-based authorization, and follows industry best practices with Clean Architecture and CQRS patterns.

## ✨ Implemented features (current)

- 🔐 Authentication & Authorization
  - ASP.NET Core Identity with a separate Identity DB (`SchoolDbContext_Identity`).
  - JWT Bearer authentication with roles (Admin, Teacher, Student).
  - Auth endpoints: register, login, get current user, map student -> user id.
  - JWT is also accepted via `access_token` query string for the SignalR chat hub.

- 👨‍🎓 Student management
  - Full CRUD: list (paginated), get by id, create (add), update, delete.
  - Device token registration endpoint for Push Notifications (stores Firebase token on the user).

- 🏫 Class management
  - Full CRUD: list (paginated), get by id, create (add), update, delete.

- 📚 Course management
  - Full CRUD: list (paginated), get by id, create (add), update, delete.

- 💬 Real-time chat
  - SignalR hub available at `/chathub`.
  - Chat API endpoints to send messages, list conversations, get paginated conversation messages, unread messages and counts, mark messages read, edit and delete messages.

- � Notifications (Firebase)
  - Notification endpoints that read and mark notifications via a Firebase-backed implementation (Realtime DB integration).
  - Sends notifications when chat events occur (message sent, edited, deleted, read).

- 🧭 Architecture and patterns
  - Clean Architecture separation (Api, Application, Domain, Infrastructure).
  - CQRS with MediatR for commands & queries.
  - AutoMapper profiles for DTO mapping.
  - Specification pattern for repository queries.

- 🛠️ Infrastructure and developer conveniences
  - EF Core 8 DbContexts: main application DB (`SchoolDbContext`) and Identity DB (`SchoolDbContext_Identity`).
  - Centralized exception handling middleware.
  - Data seeding via `IDataSeed` for both identity and application data.
  - Swagger/OpenAPI with Bearer security definition.
  - CORS policy named `AngularAppPolicy` (configured for the Angular front-end by default).


## 🏗️ Architecture

The solution follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────┐
│          School.Api (Presentation)          │
│  Controllers, Middlewares, Extensions       │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│      School.Application (Application)       │
│  Features, DTOs, MediatR, Profiles          │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│         School.Domain (Domain)              │
│  Entities, Business Rules                   │
└─────────────────────────────────────────────┘
                  ▲
┌─────────────────┴───────────────────────────┐
│    School.Infrastructure (Infrastructure)   │
│  Data Access, EF Core, Identity, Repos      │
└─────────────────────────────────────────────┘
```

### Project Layers

| Project | Responsibility |
|---------|---------------|
| **School.Api** | RESTful API endpoints, middleware, dependency injection |
| **School.Application** | Business logic, CQRS handlers, DTOs, AutoMapper profiles |
| **School.Domain** | Domain entities, business rules, interfaces |
| **School.Infrastructure** | Data persistence, EF Core, Identity, repositories |

## 🛠️ Technologies

- **Framework:** .NET 8.0
- **Database:** SQL Server with Entity Framework Core 8.0
- **Authentication:** ASP.NET Core Identity + JWT Bearer
- **Architecture Pattern:** Clean Architecture + CQRS
- **Mediator:** MediatR
- **Object Mapping:** AutoMapper
- **API Documentation:** Swagger/Swashbuckle
- **CORS:** Configured for Angular frontend

## 📦 Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer Edition)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (optional)

### Install EF Core Tools

```cmd
dotnet tool install --global dotnet-ef
```

## 🚀 Getting Started

### 1. Clone the Repository

```cmd
git clone <repository-url>
cd SchoolManagement
```

### 2. Restore Dependencies

```cmd
dotnet restore
```

### 3. Update Connection Strings

Edit `School.Api/appsettings.json` and update the connection strings if needed:

```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=localhost;Database=SchoolManagementDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "IdentityDbConnection": "Server=localhost;Database=SchoolManagementDb.Identity;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 4. Build the Solution

```cmd
dotnet build
```

### 5. Apply Database Migrations

```cmd
cd School.Infrastructure
dotnet ef database update --startup-project ../School.Api --context SchoolDbContext
dotnet ef database update --startup-project ../School.Api --context SchoolDbContext_Identity
cd ..
```

### 6. Run the Application

```cmd
cd School.Api
dotnet run
```

The API will be available at:
- **HTTP:** http://localhost:5070
- **HTTPS:** https://localhost:7208
- **Swagger UI:** https://localhost:7208/swagger

## 🗄️ Database Setup

The application uses **two separate databases**:

1. **SchoolManagementDb** - Main application data (Students, Classes, Courses, Enrollments)
2. **SchoolManagementDb.Identity** - Identity and authentication data (Users, Roles)

### Migration Commands

#### Create New Migration

```cmd
cd School.Infrastructure

# For main database
dotnet ef migrations add <MigrationName> --startup-project ../School.Api --context SchoolDbContext

# For identity database
dotnet ef migrations add <MigrationName> --startup-project ../School.Api --context SchoolDbContext_Identity
```

#### Apply Migrations

```cmd
# For main database
dotnet ef database update --startup-project ../School.Api --context SchoolDbContext

# For identity database
dotnet ef database update --startup-project ../School.Api --context SchoolDbContext_Identity
```

#### Remove Last Migration

```cmd
dotnet ef migrations remove --startup-project ../School.Api --context SchoolDbContext
```

### Data Seeding

The application includes a data seeding mechanism (`IDataSeed` interface) that automatically populates the database with initial data during development. Seed data is defined in:

- `School.Infrastructure/Implementation/DataSeed.cs`
- `School.Infrastructure/Data/SeedData/`

## 📖 API Documentation

### Base URL

```
https://localhost:7208
```

### Key API endpoints (implemented)

Authentication

- POST /api/auth/register — Register new user (AllowAnonymous)
- POST /api/auth/login — Login and retrieve JWT (AllowAnonymous)
- GET /api/auth/currentuser — Get current user (Authorized)
- GET /api/auth/GetUserId/{StudentId} — Get application user id for a student (Authorized)

Students

- GET /api/students — Get paginated students (Authorized: Admin,Teacher)
- GET /api/students/{id} — Get student by id (Authorized: Admin,Teacher,Student)
- POST /api/students/add — Create student (Authorized: Admin)
- PUT /api/students/{id} — Update student (Authorized: Admin)
- DELETE /api/students/{id} — Delete student (Authorized: Admin)
- POST /api/students/token — Register device token for push notifications (Authorized: Student)

Classes

- GET /api/classes — Get paginated classes (Authorized: Admin,Teacher,Student)
- GET /api/classes/{id} — Get class by id (Authorized: Admin,Teacher,Student)
- POST /api/classes/add — Create class (Authorized: Admin)
- PUT /api/classes/{id} — Update class (Authorized: Admin,Teacher)
- DELETE /api/classes/{id} — Delete class (Authorized: Admin)

Courses

- GET /api/courses — Get paginated courses (Authorized: Admin,Teacher,Student)
- GET /api/courses/{id} — Get course by id (Authorized: Admin,Teacher,Student)
- POST /api/courses/add — Create course (Authorized: Admin)
- PUT /api/courses/{id} — Update course (Authorized: Admin,Teacher)
- DELETE /api/courses/{id} — Delete course (Authorized: Admin)

Chat & Real-time

- SignalR hub: `/chathub` — real-time messaging hub (JWT can be passed via access_token query string).
- POST /api/chat/send-message — Send message (Authorized: Student,Teacher,Admin)
- GET /api/chat/conversations — Get user's conversations
- GET /api/chat/conversation — Get paginated messages (query params)
- GET /api/chat/unread-messages — Get unread messages
- GET /api/chat/unread-messages-count — Get unread message count
- PUT /api/chat/mark-as-read/{otherUserId} — Mark messages as read
- PUT /api/chat/edit-message — Edit a message
- DELETE /api/chat/delete-message/{messageId}?receiverId={id} — Delete a message

Notifications

- GET /api/notifications/my-notifications — Get current user's notifications (Firebase-backed)
- PUT /api/notifications/{notificationId}/mark-read — Mark a notification as read

Authentication header

Protected endpoints require a JWT token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

## 📁 Project Structure

```
SchoolManagement/
│
├── School.Api/                          # Web API Layer
│   ├── Controllers/                     # API Controllers
│   │   ├── AuthController.cs           # Authentication endpoints
│   │   ├── StudentsController.cs       # Student CRUD
│   │   ├── ClassesController.cs        # Class CRUD
│   │   └── CoursesController.cs        # Course CRUD
│   ├── Middlewares/                     # Custom middleware
│   │   └── CustomExceptionHandlerMiddleware.cs
│   ├── Extensions/                      # Service registration
│   │   ├── ServiceRegistration.cs
│   │   └── WebApplicationRegistration.cs
│   ├── ErrorModels/                     # Error DTOs
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── appsettings.json
│   └── Program.cs
│
├── School.Application/                  # Application Layer
│   ├── Features/                        # CQRS Features
│   │   ├── Auth/                       # Authentication features
│   │   │   ├── Commands/               # Register command
│   │   │   ├── Queries/                # Login, GetCurrentUser
│   │   │   └── DTOs/                   # UserDto
│   │   ├── Students/                   # Student features
│   │   ├── Classes/                    # Class features
│   │   └── Courses/                    # Course features
│   ├── MappingProfiles/                # AutoMapper profiles
│   ├── Interfaces/                     # Application interfaces
│   │   ├── Repositories/               # Repository interfaces
│   │   └── Services/                   # Service interfaces
│   ├── Common/                         # Shared application code
│   │   ├── Exceptions/                 # Custom exceptions
│   │   └── Models/                     # Common models
│   └── DependencyInjection.cs
│
├── School.Domain/                       # Domain Layer
│   └── Entities/                       # Domain entities
│       ├── BaseEntity.cs               # Base entity
│       ├── Student.cs                  # Student entity
│       ├── Class.cs                    # Class entity
│       ├── Course.cs                   # Course entity
│       └── Enrollment.cs               # Enrollment entity
│
├── School.Infrastructure/               # Infrastructure Layer
│   ├── Data/                           # Database contexts
│   │   ├── SchoolDbContext.cs          # Main DB context
│   │   ├── SchoolDbContext_Identity.cs # Identity DB context
│   │   ├── Configurations/             # Entity configurations
│   │   ├── Migrations/                 # EF migrations
│   │   └── SeedData/                   # Seed data
│   ├── Identity/                       # Identity configuration
│   │   └── ApplicationUser.cs
│   ├── Implementation/                 # Interface implementations
│   │   ├── Repositories/               # Repository implementations
│   │   ├── Services/                   # Service implementations
│   │   ├── Specifications/             # Specification pattern
│   │   └── DataSeed.cs
│   ├── Extensions/
│   │   └── SpecificationEvaluator.cs
│   └── DependencyInjection.cs
│
└── SchoolManagement.sln                 # Solution file
```

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DbConnection": "Server=localhost;Database=SchoolManagementDb;Trusted_Connection=True;TrustServerCertificate=True;",
    "IdentityDbConnection": "Server=localhost;Database=SchoolManagementDb.Identity;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "Issuer": "SchoolManagementApi",
    "Audience": "SchoolManagement",
    "SecretKey": "your-secret-key-here"
  },
  "CorsSettings": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "https://localhost:4200"
    ]
  }
}
```

### Environment Variables (Optional)

You can override settings using environment variables:

```cmd
set ConnectionStrings__DbConnection=Server=...
set JwtSettings__SecretKey=your-secret-key
```

## 👨‍💻 Development

### Running in Development Mode

```cmd
cd School.Api
dotnet run --launch-profile https
```

### Running with Hot Reload

```cmd
dotnet watch run
```

### Code Style

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML comments for public APIs
- Keep methods small and focused

### Adding New Features

1. **Domain Entity** - Add to `School.Domain/Entities/`
2. **Database Configuration** - Add to `School.Infrastructure/Data/Configurations/`
3. **Migration** - Create and apply migration
4. **DTOs** - Add to `School.Application/Features/<Feature>/DTOs/`
5. **Commands/Queries** - Add to `School.Application/Features/<Feature>/`
6. **Handlers** - Implement MediatR handlers
7. **Mapping** - Configure AutoMapper profile
8. **Controller** - Add API endpoints in `School.Api/Controllers/`

## 🐛 Troubleshooting

### Common Issues

#### 1. Database Connection Failed

**Error:** `Cannot open database...`

**Solution:** 
- Ensure SQL Server is running
- Check connection string in `appsettings.json`
- Verify database exists or run migrations

#### 2. Migration Failed

**Error:** `Unable to create an object of type 'DbContext'`

**Solution:**
```cmd
dotnet ef database update --startup-project ../School.Api --context SchoolDbContext
```

#### 3. JWT Token Invalid

**Error:** `401 Unauthorized`

**Solution:**
- Check token expiration
- Verify JWT secret key matches between token generation and validation
- Ensure `Authorization: Bearer <token>` header is set

#### 4. CORS Error

**Error:** `Access to XMLHttpRequest has been blocked by CORS policy`

**Solution:**
- Add your frontend URL to `CorsSettings.AllowedOrigins` in `appsettings.json`
- Ensure CORS middleware is registered in `Program.cs`

#### 5. Port Already in Use

**Error:** `Address already in use`

**Solution:**
- Change port in `launchSettings.json`
- Kill process using the port:
```cmd
netstat -ano | findstr :5070
taskkill /PID <PID> /F
```

### Logging

Check application logs for detailed error information. Logs are configured in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📧 Contact

For questions or support, please open an issue in the repository.

---

**Built with ❤️ using ASP.NET Core 8.0**
