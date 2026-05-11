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

        DebtOutputDto debtResult = getDebtData();
        var result = _service.getData(debtResult);

        return Ok(result);
    }



    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] DSCRDataDto dscrData)
    {
        var sessionId = getSessionId();

        if (!_cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto debtResult))
        {
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

    public DebtOutputDto? getDebtData()
    {
        var sessionId = getSessionId();
        
        return _cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto debtResult) ? debtResult : null;
    }

    public string getSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }
}