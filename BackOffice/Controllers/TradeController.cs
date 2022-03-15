using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly ILogger<TradeController> _logger;

    public TradeController(ILogger<TradeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public bool Get()
    {
        return true;
    }
}