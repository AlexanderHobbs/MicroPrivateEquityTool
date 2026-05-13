namespace DSCRCalculator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class DSCRController : ControllerBase
{
    private readonly DSCRService _service;
    private readonly IMemoryCache _cache;

    public DSCRController(DSCRService service, IMemoryCache cache) {
        _service = service;
        _cache = cache;
    }


    [HttpGet("default")]
    public IActionResult GetDscrData()
    {
        var sessionId = GetSessionId();

        if (!_cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto? debtResult))
            return Ok(new DSCROutputDto());

        var result = _service.GetData(debtResult!);
        return Ok(result);

    }



    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] DSCRDataDto dscrData)
    {
        if (dscrData == null)
            return BadRequest(new { error = "Request body is required." });


        var sessionId = GetSessionId();
        Console.WriteLine(sessionId);

        if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if (!_cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto? debtResult))
        {
            Console.WriteLine("Debt calculation not found. Please complete the debt step first.");
            return BadRequest(new
            {
                error = "Debt calculation not found. Please complete the debt step first."
            });
        }

        var result = _service.Calculate(dscrData, debtResult);

        _cache.Set($"{sessionId}:dscr_result", result, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        });

        return Ok(result);
    }

    public string GetSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }
}