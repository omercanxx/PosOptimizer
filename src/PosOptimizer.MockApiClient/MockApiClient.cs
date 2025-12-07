using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using PosOptimizer.MockApiClient.Constants;
using PosOptimizer.MockApiClient.Options;
using PosOptimizer.MockApiClient.Responses;

namespace PosOptimizer.MockApiClient;

public class MockApiClient : IMockApiClient
{
    private readonly HttpClient _httpClient;

    public MockApiClient(HttpClient httpClient, IOptions<MockApiOptions> options)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
    }

    public async Task<List<PosRatioDto>> GetRatiosAsync()
    {
        var options = new JsonSerializerOptions();
        
        options.Converters.Add(new JsonStringEnumConverter());
        
        var result = await _httpClient.GetFromJsonAsync<List<PosRatioDto>>(MockApiUrls.GetRatios, options);
        
        return result ?? new List<PosRatioDto>();
    }
}