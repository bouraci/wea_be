using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WEA_BE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingTestController : ControllerBase
    {
        private readonly ILogger<LoggingTestController> _logger;

        public LoggingTestController(ILogger<LoggingTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("info")]
        public IActionResult LogInformation()
        {
            _logger.LogInformation("This is an Information log for testing.");
            return Ok("Information log has been recorded.");
        }

        [HttpGet("warning")]
        public IActionResult LogWarning()
        {
            _logger.LogWarning("This is a Warning log for testing.");
            return Ok("Warning log has been recorded.");
        }

        [HttpGet("error")]
        public IActionResult LogError()
        {
            _logger.LogError("This is an Error log for testing.");
            return Ok("Error log has been recorded.");
        }
    }
}
