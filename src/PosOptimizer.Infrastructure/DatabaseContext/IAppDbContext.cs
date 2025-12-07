using Microsoft.EntityFrameworkCore;
using PosOptimizer.Infrastructure.Entities;

namespace PosOptimizer.Infrastructure.DatabaseContext;

public interface IAppDbContext
{
    DbSet<PosRatioEntity> PosRatios { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}