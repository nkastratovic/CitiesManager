# CitiesManager

Web API application, ASP .NET Core 7

Entity Framework Core

Open API 3/Swagger

API Versions

Clean Architecture

MSSQL Database

PostgreSql supported

SQLite supported


--------------------------------------------------------------------------------------------------
EF Migrations

SqlServer

Add-Migration Initial -Context SqlServerDbContext -OutputDir Migrations/SqlServerDbMigrations

Update-Database -Context SqlServerDbContext


PostgreSql

Add-Migration Initial -Context PostgreSqlDbContext -OutputDir Migrations/PostgreSqlDbMigrations

Update-Database -Context PostgreSqlDbContext


SQLite

Add-Migration Initial -Context SqliteDbContext -OutputDir Migrations/SqliteDbMigrations

Update-Database -Context SqliteDbContext
