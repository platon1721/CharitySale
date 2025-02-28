using BLL.DTO;
using BLL.Exceptions;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApp.ApiControllers;

[ApiController]
[Route("api/[controller]")]
[SwaggerTag("Receipts")]
public class ReceiptsController : ControllerBase
{
   private readonly IReceiptService _receiptService;

   public ReceiptsController(IReceiptService receiptService)
   {
       _receiptService = receiptService;
   }

   [HttpGet]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Get all receipts", 
       Description = "Retrieves a list of all receipts in the system")]
   public async Task<ActionResult<List<ReceiptDto>>> GetAll()
   {
       var receipts = await _receiptService.GetAllAsync();
       return Ok(receipts);
   }
   
   // [HttpGet("open")]
   // [ProducesResponseType(StatusCodes.Status200OK)]
   // [SwaggerOperation(
   //     Summary = "Get open receipts", 
   //     Description = "Retrieves a list of open receipts for a user")]
   // public async Task<ActionResult<List<ReceiptDto>>> GetOpenReceipts(
   //     [FromQuery] int userId)
   // {
   //     var openReceipts = await _receiptService.GetOpenReceiptsAsync(userId);
   //     return Ok(openReceipts);
   // }
   
   [HttpGet("user/{userId}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [SwaggerOperation(
       Summary = "Get receipts for a specific user", 
       Description = "Retrieves a list of receipts for a given user")]
   public async Task<ActionResult<List<ReceiptDto>>> GetReceiptsOfUser(int userId)
   {
       var userReceipts = await _receiptService.GetReceiptsOfUserAsync(userId);
       return Ok(userReceipts);
   }

   [HttpGet("{id}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Get receipt by ID", 
       Description = "Retrieves a specific receipt by its unique identifier")]
   public async Task<ActionResult<ReceiptDto>> GetById(
       [SwaggerParameter("Unique identifier of the receipt")] int id)
   {
       var receipt = await _receiptService.GetByIdAsync(id);
       return Ok(receipt);
   }

   [HttpPost]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [SwaggerOperation(
       Summary = "Create a new receipt", 
       Description = "Creates a new empty receipt, that would be modified in future")]
   public async Task<ActionResult<ReceiptDto>> Create([FromBody] CreateReceiptDto dto)
   {
       try
       {
           // Lisage silumine
           Console.WriteLine($"CreateReceipt - Received UserId: {dto.UserId}");
        
           var receipt = await _receiptService.CreateAsync(dto);
        
           // Lisage silumine
           Console.WriteLine($"CreateReceipt - Created Receipt ID: {receipt.Id}");
        
           return CreatedAtAction(nameof(GetById), new { id = receipt.Id }, receipt);
       }
       catch (Exception ex)
       {
           // Täpsem veateate logimine
           Console.WriteLine($"CreateReceipt - Error: {ex.Message}");
           Console.WriteLine($"Stack trace: {ex.StackTrace}");
        
           return StatusCode(500, new { 
               message = "An error occurred while creating the receipt.", 
               details = ex.Message 
           });
       }
   }

   [HttpPut("{id}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Update an existing receipt", 
       Description = "Updates an existing receipt with the provided details")]
   public async Task<ActionResult<ReceiptDto>> Update(
       [SwaggerParameter("Unique identifier of the receipt to update")] int id, 
       [SwaggerParameter("Updated receipt details")] [FromBody] CreateReceiptDto dto)
   {
       var receipt = await _receiptService.UpdateAsync(id, dto);
       return Ok(receipt);
   }

   [HttpDelete("{id}")]
   [ProducesResponseType(StatusCodes.Status204NoContent)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Delete a receipt", 
       Description = "Deletes a specific receipt by its unique identifier")]
   public async Task<IActionResult> Delete(
       [SwaggerParameter("Unique identifier of the receipt to delete")] int id)
   {
       try
       {
           Console.WriteLine($"DeleteReceiptAPI:{id}");
            await _receiptService.DeleteAsync(id);
            return NoContent();
       }
       catch (Exception e)
       {
           Console.WriteLine(e);
           throw;
       };
   }
   
   [HttpPost("{id}/restore-stock")]
   [ProducesResponseType(StatusCodes.Status201Created)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [SwaggerOperation(
       Summary = "Delete a receipt that is finished", 
       Description = "Returns all products back that are in the receipt")]
   public async Task<IActionResult> RestoreStock(
       [SwaggerParameter("Unique identifier of the receipt to delete")] int id)
   {
       try
       {
           await _receiptService.RestoreStockAsync(id);
           return NoContent();
       }
       catch (NotFoundException ex)
       {
           return NotFound(ex.Message);
       }
       catch (Exception ex)
       {
           return BadRequest(ex.Message);
       }
   }

   [HttpPost("{receiptId}/products")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Add product to receipt", 
       Description = "Adds a new product to an existing receipt")]
   public async Task<ActionResult<ReceiptDto>> AddProductToReceipt(
       [SwaggerParameter("Unique identifier of the receipt")] int receiptId, 
       [SwaggerParameter("Product details to add")] [FromBody] AddProductToReceiptDto dto)
   {
       var receipt = await _receiptService.AddProductToReceiptAsync(receiptId, dto);
       return Ok(receipt);
   }

   [HttpPut("{receiptId}/products/{productId}")]
   [ProducesResponseType(StatusCodes.Status200OK)]
   [ProducesResponseType(StatusCodes.Status400BadRequest)]
   [ProducesResponseType(StatusCodes.Status404NotFound)]
   [SwaggerOperation(
       Summary = "Update product quantity in receipt", 
       Description = "Updates the quantity of a specific product in a receipt")]
   public async Task<ActionResult<ReceiptDto>> UpdateProductQuantity(
       [SwaggerParameter("Unique identifier of the receipt")] int receiptId, 
       [SwaggerParameter("Unique identifier of the product")] int productId, 
       [SwaggerParameter("New product quantity details")] [FromBody] UpdateReceiptProductQuantityDto dto)
   {
       var receipt = await _receiptService.UpdateProductQuantityAsync(receiptId, productId, dto);
       return Ok(receipt);
   }
}