using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/Receipts/{receiptId}/products")]
public class ProductReceiptController : ControllerBase
{
    private readonly IProductReceiptService _productReceiptService;

    public ProductReceiptController(IProductReceiptService productReceiptService)
    {
        _productReceiptService = productReceiptService;
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult<ReceiptDto>> UpdateProductQuantity(
        int receiptId, 
        int productId, 
        [FromBody] UpdateReceiptProductDto dto)
    {
        var receipt = await _productReceiptService.UpdateProductQuantityAsync(receiptId, productId, dto);
        return Ok(receipt);
    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult<ReceiptDto>> RemoveProduct(int receiptId, int productId)
    {
        var receipt = await _productReceiptService.RemoveProductAsync(receiptId, productId);
        return Ok(receipt);
    }
}