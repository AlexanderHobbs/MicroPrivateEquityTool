namespace DebtPaymentCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


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

    [HttpPost("calculate")]

    public IActionResult Calculate([FromBody] DebtDataDto debtData)
    {   

        var result = _service.Calculate(debtData);
        var sessionId = Request.Headers["X-Session-Id"].ToString();

        _cache.Set($"{sessionId}:debt_result", result, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        });

        return Ok(result);
    }

}

//    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
//             debtData, 
//             new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
//         ));