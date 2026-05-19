using Microsoft.Extensions.Caching.Memory;
using Shared.DTOs;

namespace Shared.Services;


public class CacheService
{
    private readonly IMemoryCache _cache;


    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public DebtOutputDto cacheDebtData(string sessionId)
    {
        _cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto? debtInput);
        
        return debtInput;
    }

    public DSCROutputDto cacheDSCR(string sessionId)
    {
        _cache.TryGetValue($"{sessionId}:dscr_result", out DSCROutputDto? dscr);
        
        return dscr;
    }

    public StressTestOutputDto cacheSTest(string sessionId)
    {
        _cache.TryGetValue($"{sessionId}:S-Test_result", out StressTestOutputDto st);

        return st;
    }

}