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

    [HttpGet]
    public bool Get()
    {
        return true;
    }
}