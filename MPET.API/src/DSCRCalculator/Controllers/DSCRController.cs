namespace DSCRCalculator;

[ApiController]
[Route("api/[controller]")]
public class DSCRController : ControllerBase
{
    private readonly DSCRService _service;
    private readonly IMemoryCache _cache;

    public IActionResult Calculate([FromBody] DSCRDataDto dscrData)
    {
        
    }
}