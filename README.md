# Tic-Tac-Toe Project

Welcome to the **Tic-Tac-Toe** project! This is an ASP.NET Core application built using a multi-layered architecture, featuring single-player (vs. AI) and multiplayer modes. It uses Entity Framework Core for database interactions, storing the game state in a SQL Server database.

## Table of Contents
- [Project Structure](#project-structure)
- [Setup Instructions](#setup-instructions)
  - [Clone the Repository](#1-clone-the-repository)
  - [Restore Dependencies](#2-restore-dependencies)
  - [Configure the Database](#3-configure-the-database)
  - [Install Required Packages](#4-install-required-packages)
  - [Apply Database Migrations](#5-apply-database-migrations)
  - [Build and Run](#6-build-and-run)
- [Troubleshooting](#troubleshooting)
- [Notes](#notes)

## Project Structure

The solution consists of four main projects, each serving a distinct purpose:

### `Tic-Tac-Toe.Domain`
- Contains core entities like `Game` and business logic, such as rules, turns, win conditions, and AI behavior.

### `Tic-Tac-Toe.Application`
- Provides services and interfaces to connect domain logic with infrastructure, such as `IGameService` and `GameService`.

### `Tic-Tac-Toe.Infrastructure`
- The data access layer using Entity Framework Core. Includes the `GameDbContext` and `GameRepository`.

### `Tic-Tac-Toe.Web`
- The front-end ASP.NET Core MVC web application, offering the user interface (UI) for the game board, mode selection, and game reset functionality.

## Setup Instructions

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone https://github.com/your-repo/Tic-Tac-Toe.git
cd Tic-Tac-Toe
```
> Replace `https://github.com/your-repo/Tic-Tac-Toe.git` with your repository URL.

### 2. Restore Dependencies

Make sure you have the .NET SDK installed (version 8.0 recommended). Restore NuGet packages by running:

```bash
dotnet restore
```

### 3. Configure the Database

Update the connection string in `Tic-Tac-Toe.Web/appsettings.json` to match your SQL Server setup:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TicTacToeDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```
Adjust the `Server` field according to your SQL Server instance (local or remote).

### 4. Install Required Packages

Ensure all required Entity Framework Core packages are installed:

For **Tic-Tac-Toe.Web** (to fix migration errors):
```bash
dotnet add Tic-Tac-Toe.Web package Microsoft.EntityFrameworkCore.Design --version 8.0.0
```

For **Tic-Tac-Toe.Infrastructure** (to avoid package version conflicts):
```bash
dotnet add Tic-Tac-Toe.Infrastructure package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add Tic-Tac-Toe.Infrastructure package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add Tic-Tac-Toe.Infrastructure package Microsoft.EntityFrameworkCore.Relational --version 8.0.0
```

### 5. Apply Database Migrations

You need to create and apply the initial database schema:

#### Using Visual Studio PMC:
- Set **Tic-Tac-Toe.Web** as the startup project.
- Open Package Manager Console (PMC) and set the default project to **Tic-Tac-Toe.Infrastructure**.
- Run the following commands:

```bash
PM> Add-Migration Init
PM> Update-Database
```

#### Using .NET CLI:
```bash
dotnet ef migrations add Init --project Tic-Tac-Toe.Infrastructure --startup-project Tic-Tac-Toe.Web
dotnet ef database update --project Tic-Tac-Toe.Infrastructure --startup-project Tic-Tac-Toe.Web
```

### 6. Build and Run

To build and launch the application:

```bash
dotnet build
dotnet run --project Tic-Tac-Toe.Web
```

Navigate to [https://localhost:5001/Game/Index](https://localhost:5001/Game/Index) (or a similar URL) in your browser to start playing.

## Troubleshooting

### 1. Missing Microsoft.EntityFrameworkCore.Design Error
**Error**: "Your startup project 'Tic-Tac-Toe.Web' doesn't reference `Microsoft.EntityFrameworkCore.Design`."

**Fix**: Install the required package in `Tic-Tac-Toe.Web` by following step 4.

### 2. Package Downgrade Warning
**Warning**: "Detected package downgrade: `Microsoft.EntityFrameworkCore` from 9.0.2 to 8.0.0."

**Fix**: Ensure all EF Core packages use the same version, e.g., 8.0.0. Check `.csproj` files and align versions.

### 3. Migration Assembly Mismatch
**Error**: Migrations fail due to assembly mismatch.

**Fix**: In `Tic-Tac-Toe.Web/Program.cs`, add the following to resolve the mismatch:

```csharp
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Tic-Tac-Toe.Infrastructure")));
```

Feel free to reach out for further assistance.
