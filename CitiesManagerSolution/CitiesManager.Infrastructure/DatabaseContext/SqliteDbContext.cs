using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
 public class SqliteDbContext : ApplicationDbContext
 {
  public SqliteDbContext(DbContextOptions options) : base(options)
  {
  }

  public SqliteDbContext()
  {
  }
  /// <inheritdoc/>
  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("Data Source=SqliteDb.db;");
 }
}
