.NET 8 Security Module with Onion Architecture
This project implements a secure authentication and authorization module in .NET 8, following Onion Architecture principles. It provides a foundation for user login, registration, and data management using SQL Server.

Features
Onion Architecture: Structured into layers (Presentation, Application Services, Domain, Infrastructure) for modularity and maintainability.
JWT Authentication: Utilizes JSON Web Tokens for secure user authentication.
Login and Registration: Simple implementations for user account management.
SQL Server Integration: Stores user data securely in a SQL Server database.
Dependency Injection: Uses .NET's built-in DI for managing dependencies and promoting testability.
Technologies Used
.NET 8: Latest version of the .NET framework.
C#: Primary language for development.
SQL Server: Relational database for persistent data storage.
JWT: Token-based authentication mechanism.
Getting Started
Prerequisites
.NET 8 SDK
SQL Server


Configuration
Open appsettings.json and configure your SQL Server connection string.
"ConnectionStrings": {
  "IdentityConnection": "Server=.;Database=Security.Module;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
}

Build and Run
Restore dependencies: dotnet restore
Build the project: dotnet build
Run the project: dotnet run
