namespace Core.Persistence.Data;
public class AppDbContext : DbContext
{
    public DbSet<WorkLog> WorkLogs => Set<WorkLog>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
}