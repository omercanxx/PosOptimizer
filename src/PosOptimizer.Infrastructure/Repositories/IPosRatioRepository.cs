using PosOptimizer.Infrastructure.Entities;

namespace PosOptimizer.Infrastructure.Repositories;

public interface IPosRatioRepository
{
    Task AddAsync(List<PosRatioEntity> entites);
}