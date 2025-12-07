using PosOptimizer.MockApiClient.Responses;

namespace PosOptimizer.MockApiClient;

public interface IMockApiClient
{
    Task<List<PosRatioDto>> GetRatiosAsync();
}