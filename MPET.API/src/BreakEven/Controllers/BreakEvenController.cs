using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BreakEvenCalculator;

[ApiController]
[Route("api/[controller]")]

public class BreakEvenController : ControllerBase
{
    private readonly BreakEvenService _service;
    private readonly IMemoryCache _cache;

    public BreakEvenController(BreakEvenService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    [HttpPost("calculate")]
    public IActionResult calculate([FromBody] BreakEvenDto beDto)
    {
       if (beDto == null)
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

        _cache.Set($"{sessionId}:breakeven_input", beDto, options);


        var result = _service.Calculate(beDto);

        _cache.Set($"{sessionId}:breakeven_result", result, options);

        return Ok(result);

    }

    private string GetSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }

}