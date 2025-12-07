using PosOptimizer.Infrastructure.DatabaseContext;
using PosOptimizer.Infrastructure.Entities;

namespace PosOptimizer.Infrastructure.Repositories;

public class PosRatioRepository : IPosRatioRepository
{
    private readonly IAppDbContext _dbContext;
    
    public PosRatioRepository(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(List<PosRatioEntity> entities)
    {
        await _dbContext.PosRatios.AddRangeAsync(entities);
        
        await  _dbContext.SaveChangesAsync();
    }
}