using EFModels.Data;
using Microsoft.AspNetCore.Mvc;
using WEA_BE.Models;
using WEA_BE.Services;

namespace WEA_BE.Controllers;
[Route("data")]
[ApiController]
public class DataLoadController : ControllerBase
{
    private readonly ILogger<DataLoadController> _logger;
    private readonly DatabaseContext _ctx;
    private readonly FilePathOptions _options;

    public DataLoadController(ILogger<DataLoadController> logger, DatabaseContext ctx, FilePathOptions options)
    {
        _logger = logger;
        _ctx = ctx;
        _options = options;
    }

    // Endpoint to load data from CSV file
    [HttpPost("csv")]
    public async Task<IActionResult> LoadFromCsv()
    {
        try
        {
            await LoadFromCSVService.LoadFromCSV(_options.CsvPath, _ctx);
            return Ok("Data loaded successfully from CSV file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading data from CSV.");
            return StatusCode(500, "Internal server error.");
        }
    }

    // Endpoint to load data from string (passed in request body)
    [HttpPost("cdb")]
    public async Task<IActionResult> LoadFromString([FromBody] string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return BadRequest("No data provided.");
        }

        try
        {
            await LoadFromStringService.LoadFromString(_ctx, data);
            return Ok("Data loaded successfully from string.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while loading data from string.");
            return StatusCode(500, "Internal server error.");
        }
    }
}
