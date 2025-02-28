using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;


/// <summary>
/// Data Transfer Object (DTO) for updating the quantity of a product in a receipt.
/// </summary>
public class UpdateReceiptProductDto
{
    
    /// <summary>
    /// Gets or sets the ID of the product to be updated.
    /// </summary>
    [Required]
    public int ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the updated quantity of the product.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}