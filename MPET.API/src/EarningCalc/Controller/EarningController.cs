namespace EarningCalculator;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MPET.Controller;
using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class EarningController : ControllerBase
{

    private readonly EarningService _service;
    private readonly IMemoryCache _cache;

    public EarningController(EarningService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpGet("default")]

    public IActionResult RetrieveCalcData()
    {
        var sessionId = GetSessionId();

        if (!_cache.TryGetValue($"{sessionId}:earning_result", out EarningOutputDto? eaResult))
            return Ok(new EarningOutputDto());

        return Ok(eaResult);

    }

    [HttpPost("calculate")]

    public IActionResult calculate([FromBody] EarningCalculatorDto eaDto)
    {
        if (eaDto == null)
            return BadRequest(new { error = "Request body is required." });

        var sessionId = GetSessionId();

        if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }

        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(30)
        };

        _cache.Set($"{sessionId}:earning_input", eaDto, options);

        var result = _service.Calculate(eaDto);

        _cache.Set($"{sessionId}:earning_result", result, options);      
            
        return Ok(result);
    }

    public string GetSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }


    
}
