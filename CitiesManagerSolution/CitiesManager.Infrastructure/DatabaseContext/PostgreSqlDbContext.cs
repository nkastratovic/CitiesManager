using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
 public class PostgreSqlDbContext : ApplicationDbContext
 {
  public PostgreSqlDbContext(DbContextOptions options) : base(options)
  {
  }

  public PostgreSqlDbContext()
  {
  }
  /// <inheritdoc/>
  protected override void OnConfiguring(DbContextOptionsBuilder options)
  {
   options.UseNpgsql("User ID=postgres;Password=123456;Host=localhost;Port=5432;Database=CitiesDatabase;")
    .UseSnakeCaseNamingConvention();
  }
 }
}
