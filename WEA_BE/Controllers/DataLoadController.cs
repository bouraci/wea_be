using EFModels.Data;
using Microsoft.AspNetCore.Mvc;
using WEA_BE.Models;
using WEA_BE.Services;

namespace WEA_BE.Controllers;
[Route("data")]
[ApiController]
public class DataLoadController : ControllerBase
{
    private readonly ILogger<BooksController> _logger;
    private readonly DatabaseContext _ctx;
    private readonly FilePathOptions _options;
    public DataLoadController(ILogger<BooksController> logger, DatabaseContext ctx, FilePathOptions options)
    {
        _logger = logger;
        _ctx = ctx;
        _options = options;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        if (await LoadFromApiService.TryLoadFromApi(_ctx, _logger) == false)
        {
            if (!_ctx.Books.Any())
            {
                await LoadFromCSVService.LoadFromCSV(_options.CsvPath, _ctx);
            }
        }
        return Ok();
    }
}
