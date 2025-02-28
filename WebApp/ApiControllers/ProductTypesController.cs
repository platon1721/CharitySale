using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Product Types")]
public class ProductTypesController : ControllerBase
{
    private readonly IProductTypeService _productTypeService;

    public ProductTypesController(IProductTypeService productTypeService)
    {
        _productTypeService = productTypeService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get all product types", Description = "Retrieves a list of all product types")]
    public async Task<ActionResult<List<ProductTypeDto>>> GetAll()
    {
        var productTypes = await _productTypeService.GetAllAsync();
        return Ok(productTypes);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductTypeDto>> GetById(int id)
    {
        var productType = await _productTypeService.GetByIdAsync(id);
        return Ok(productType);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Create productType", Description = "Create a new productType")]
    public async Task<ActionResult<ProductTypeDto>> Create([FromBody] CreateProductTypeDto dto)
    {
        var createdProductType = await _productTypeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdProductType.Id }, createdProductType);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update productType", Description = "Update the productType")]
    public async Task<ActionResult<ProductTypeDto>> Update(int id, [FromBody] CreateProductTypeDto dto)
    {
        var updatedProductType = await _productTypeService.UpdateAsync(id, dto);
        return Ok(updatedProductType);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Delete productType", Description = "Delete the productType")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productTypeService.DeleteAsync(id);
        return NoContent();
    }
}