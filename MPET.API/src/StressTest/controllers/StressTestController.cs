using EarningCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Shared.DTOs;
using Shared.Services;

namespace StressTestCalculator;


[ApiController]
[Route("api/[controller]")]
public class StressTestController : ControllerBase
{

    private readonly StressTestService _service;
    private readonly IMemoryCache _cache;

    private readonly CacheService _cacheService;

    public StressTestController(StressTestService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpGet("default")]
    public IActionResult getDefaultValues()
    {
        var sessionId = GetSessionId();

        // _cache.TryGetValue($"{sessionId}:earning_result", out EarningOutputDto earningResult);
        _cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto? debtResult);
        _cache.TryGetValue($"{sessionId}:dscr_result", out DSCROutputDto dscrResult);
        _cache.TryGetValue($"{sessionId}:earning_input", out EarningCalculatorDto eaDto);

        StressTestModel st = new(){
            NerveLevel = 5,
            Revenue = 100,
            Expense = 200,
            RevenueDrop = 0,
            MarginLevelShift = 0,
            InterestRateShift = 5
        };

        return Ok(st);
    }


    [HttpPost("calculate")]
    public IActionResult calculate([FromBody] StressTestDto? stDto)
    {
        try {

        Console.WriteLine($"→ stModel is null: {stDto == null}");

        var sessionId = GetSessionId();
        
        _cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto? db);


        if (db == null)
        {
            return BadRequest("Debt data not found. Please complete the debt calculator first.");
        }

        StressTestModel stModel = stDto.STModel;
        
        var result = _service.calculate(stModel, db);

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        _cache.Set($"{sessionId}:STest_result", result, options);
        _cache.Set($"{sessionId}:STest_input", stModel, options);

        return Ok(result);
    } catch (System.Exception ex) {
        // This stops the 500 error from hiding the details and dumps the C# crash logs into your network tab
        return StatusCode(500, new { errorMessage = ex.Message, innerError = ex.InnerException?.Message, stack = ex.StackTrace });
    }

    }

    public string GetSessionId()
    {
        return Request.Headers["X-Session-Id"].ToString();
    }

}