using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastructure.DatabaseContext
{
 public class SqlServerDbContext : ApplicationDbContext
 {
  public SqlServerDbContext(DbContextOptions options) : base(options)
  {
  }

  public SqlServerDbContext()
  {
  }
  /// <inheritdoc/>
  protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CitiesDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
 }
}
