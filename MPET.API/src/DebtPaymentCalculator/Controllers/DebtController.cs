namespace DebtPaymentCalculator;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DebtController : ControllerBase
{
    private readonly DebtService _service;

    public DebtController(DebtService service)
    {
        _service = service;
    }

    [HttpPost("calculate")]

    public IActionResult Calculate([FromBody] DebtDataDto debtData)
    {   

        var result = _service.Calculate(debtData);

        return Ok(result);
    }

}

//    Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(
//             debtData, 
//             new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
//         ));