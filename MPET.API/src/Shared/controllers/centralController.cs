using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
namespace MPET.Controller;

using Shared.DTOs;

[ApiController]
[Route("api/[controller]")]
public class CentralController : ControllerBase
{

    private readonly IMemoryCache _cache;

    public CentralController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet("earning")]
    public IActionResult GetEarningResults()
    {

        var sessionId = GetSessionId();

         if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if(!_cache.TryGetValue($"{sessionId}:earning_result", out EarningOutputDto results))
        {
            return BadRequest(new {error = "No earning service history-Service Controller possibly not yet been envoked"});
        }

        return Ok(results);
    }

    [HttpGet("debt")]
    public IActionResult GetDebtResults()
    {

        var sessionId = GetSessionId();

         if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if(!_cache.TryGetValue($"{sessionId}:debt_result", out DebtOutputDto results))
        {
            return BadRequest(new {error = "No debt service history-Service Controller possibly not yet been envoked"});
        }

        return Ok(results);
    }


    [HttpGet("dscr")]
    public IActionResult GetDSCRResults()
    {

        var sessionId = GetSessionId();

         if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if(!_cache.TryGetValue($"{sessionId}:dscr_result", out DSCROutputDto results))
        {
            return BadRequest(new {error = "No dscr service history-Service Controller possibly not yet been envoked"});
        }

        return Ok(results);
    }

    [HttpGet("stest")]
    public IActionResult GetSTestResults()
    {

        var sessionId = GetSessionId();

         if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if(!_cache.TryGetValue($"{sessionId}:STest_result", out DSCROutputDto results))
        {
            return BadRequest(new {error = "No stress test service history-Service Controller possibly not yet been envoked"});
        }

        return Ok(results);
    }

    [HttpGet("breakeven")]
    public IActionResult GetBreakEvenResults()
    {

        var sessionId = GetSessionId();

         if (string.IsNullOrEmpty(sessionId)){
            return BadRequest(new { error = "X-Session-Id header is required." });
        }


        if(!_cache.TryGetValue($"{sessionId}:breakeven_result", out BreakEvenOutputDto results))
        {
            return BadRequest(new {error = "No stress test service history-Service Controller possibly not yet been envoked"});
        }

        return Ok(results);
    }



    private string GetSessionId()
    {
            return Request.Headers["X-Session-Id"].ToString();
    }

    //generate session id from vue frontend
    //create cache and populate with all new objects for each class.

}