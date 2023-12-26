# CitiesManager
Web API application, ASP .NET Core 7
Entity Framework Core
Open API 3/Swagger
API Versions
Clean Architecture
MSSQL Database




--------------------------------------------------------------------------------------------------
EF Migrations

SqlServer
Add-Migration Initial -Context SqlServerDbContext -OutputDir SqlServerDbMigrations
Update-Database -Context SqlServerDbContext

PostgreSql
Add-Migration Initial -Context PostgreSqlDbContext -OutputDir PostgreSqlDbMigrations
Update-Database -Context PostgreSqlDbContext

Sqlite
Add-Migration Initial -Context SqliteDbContext -OutputDir SqliteDbMigrations
Update-Database -Context SqliteDbContext