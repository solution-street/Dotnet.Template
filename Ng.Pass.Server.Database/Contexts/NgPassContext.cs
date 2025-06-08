using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ng.Pass.Server.Core.Configuration;
using Ng.Pass.Server.Database.Entities;
using Ng.Pass.Server.Database.Interfaces;

namespace Ng.Pass.Server.Database.Contexts;

public partial class NgPassContext : DbContext
{
    public NgPassContext() { }

    public NgPassContext(DbContextOptions<NgPassContext> options)
        : base(options) { }

    public virtual DbSet<Password> Passwords { get; set; }

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
