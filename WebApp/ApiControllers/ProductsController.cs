using BLL.DTO;
using BLL.Exceptions;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ProductsController(IProductService productService, IWebHostEnvironment hostingEnvironment)
    {
        _productService = productService;
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get all products", Description = "Retrieves a list of all products")]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get all active products", Description = "Retrieves a list of all products that were not deleted")]
    public async Task<ActionResult<List<ProductDto>>> GetAllActive()
    {
        var products = await _productService.GetAllActiveAsync();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get product", Description = "Retrieves a product by Id")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Create product", Description = "Create a new product")]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto productDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = await _productService.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }
    

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update product", Description = "Update the product")]
    public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] CreateProductDto dto)
    {
        var updatedProduct = await _productService.UpdateAsync(id, dto);
        return Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Delete product", Description = "Delete the product")]
    public async Task<ActionResult<ProductDto>> Delete(int id)
    {
        Console.WriteLine("Deleting product");
        Console.WriteLine(id);
        await _productService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpGet("stock-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get stock status", Description = "Retrieves a stock status of products")]
    public async Task<ActionResult<List<ProductStockDto>>> GetStockStatus([FromQuery] List<int> productIds)
    {
        if (productIds == null || !productIds.Any())
        {
            return BadRequest("Product IDs must be provided");
        }
        try 
        {
            var stockStatuses = await _productService.GetProductsStockStatusAsync(productIds);
            
            if (!stockStatuses.Any())
            {
                return NotFound("No products found for the given IDs");
            }

            return Ok(stockStatuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching stock status");
        }
    }
    
    [HttpPut("update-stock")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Update product stock", Description = "Updates the stock of a product")]
    public async Task<ActionResult<ProductDto>> UpdateStock([FromBody] AddProductToReceiptDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try 
        {
            var updatedProduct = await _productService.UpdateStockAsync(dto.ProductId, -dto.Quantity);
            return Ok(updatedProduct);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while updating stock: {ex.Message}");
        }
    }
}