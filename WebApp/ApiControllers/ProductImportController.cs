using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Product Import")]
public class ProductImportController : ControllerBase
{
    private readonly IProductImportService _importService;

    public ProductImportController(IProductImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Import products from a configuration file",
        Description = "Uploads and processes a JSON configuration file containing product information")]
    public async Task<IActionResult> ImportFromFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        if (!file.FileName.EndsWith(".json"))
        {
            return BadRequest("Only JSON files are supported.");
        }

        var result = await _importService.ImportProductsFromFileAsync(file);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("from-default")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Import products from the default configuration file",
        Description = "Processes the default JSON configuration file located in the Config folder")]
    public async Task<IActionResult> ImportFromDefaultConfig()
    {
        var result = await _importService.ImportProductsFromDefaultConfigAsync();

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}