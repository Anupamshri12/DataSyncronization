using Microsoft.EntityFrameworkCore;
using WebApplication4.Entities;

namespace WebApplication4.ApplicationDBContext;

public class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {
    }

    //public DbSet<UserModel> User { get; set; }

    public DbSet<TaskTable> TaskTable { get; set; }
    
}

public class LocalDbContext : DbContext
{
    public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options)
    {
    }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<SyncQueue> SyncQ { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TaskEntity>().ToTable("Tasks");
        modelBuilder.Entity<SyncQueue>().ToTable("SyncQueue");
    }
}



