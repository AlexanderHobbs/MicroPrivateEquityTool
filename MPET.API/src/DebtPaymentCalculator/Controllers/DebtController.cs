namespace DebtPaymentCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MPET.Controller;
using Shared.DTOs;


[ApiController]
[Route("api/[controller]")]
public class DebtController : ControllerBase
{
    private readonly DebtService _service;
    private readonly IMemoryCache _cache;


    public DebtController(DebtService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpGet("default")]
    public IActionResult GetDebtData()
    {
        var sessionId = GetSessionId();

        if (string.IsNullOrEmpty(sessionId))
            return Ok(new DebtDataDto());

        if (!_cache.TryGetValue($"{sessionId}:debt_input", out DebtDataDto? debtInput))
            return Ok(new DebtDataDto());

        
        return Ok(debtInput); 
    }


    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] DebtDataDto debtData)
    {   

        var sessionId = GetSessionId();

        var result = _service.Calculate(debtData);

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        _cache.Set($"{sessionId}:debt_result", result, options);
        _cache.Set($"{sessionId}:debt_input", debtData, options);

        return Ok(result);
    }

    private string GetSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }

}

//    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
//             debtData, 
//             new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
//         ));