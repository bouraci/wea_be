using Microsoft.AspNetCore.Mvc;

namespace WEA_BE.Controllers;

[ApiController]
[Route("greeting")]
public class GreetingController : ControllerBase
{

    private readonly ILogger<GreetingController> _logger;

    public GreetingController(ILogger<GreetingController> logger)
    {
        _logger = logger;
    }

    [HttpPost(Name = "GetGreeting")]
    public string Get([FromBody] string name)
    {
        _logger.LogInformation($"Said hello to {name}");
        return $"Hello, {name}";
    }
}