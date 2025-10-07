using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Dotnet.Template.Server.Core.Configuration;
using Dotnet.Template.Server.Database.Entities;
using Dotnet.Template.Server.Database.Interfaces;

namespace Dotnet.Template.Server.Database.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public virtual DbSet<Secret> Secrets { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This is needed for the EF Core CLI tools
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().Build();
            var connectionString = configuration.GetConnectionString(AppConstants.Secrets.ConnectionStringKey);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public override int SaveChanges()
    {
        SetTimeAuditedColumns();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetTimeAuditedColumns();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetTimeAuditedColumns()
    {
        var entitiesCreated = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ITimestampedEntity && e.State == EntityState.Added)
            .Select(x => x.Entity as ITimestampedEntity);

        var entitiesModified = ChangeTracker
            .Entries()
            .Where(e => e.Entity is ITimestampedEntity && (e.State == EntityState.Added || e.State == EntityState.Modified))
            .Select(x => x.Entity as ITimestampedEntity);

        foreach (var entity in entitiesCreated)
        {
            entity!.CreatedAt = DateTime.UtcNow;
        }

        foreach (var entity in entitiesModified)
        {
            entity!.UpdatedAt = DateTime.UtcNow;
        }
    }
}
