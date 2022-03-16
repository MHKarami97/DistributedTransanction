using Microsoft.AspNetCore.Mvc;

namespace Accounting.Controllers;

[ApiController]
[Route("[controller]")]
public class MoneyController : ControllerBase
{
    private readonly ILogger<MoneyController> _logger;

    public MoneyController(ILogger<MoneyController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public bool Block()
    {
        return true;
    }
    
    [HttpPost]
    public bool UnBlock()
    {
        return true;
    }
}