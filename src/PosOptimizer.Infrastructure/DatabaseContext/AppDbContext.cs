using Microsoft.EntityFrameworkCore;
using PosOptimizer.Infrastructure.Entities;

namespace PosOptimizer.Infrastructure.DatabaseContext;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<PosRatioEntity> PosRatios => Set<PosRatioEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasKey(nameof(BaseEntity.Id));
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}