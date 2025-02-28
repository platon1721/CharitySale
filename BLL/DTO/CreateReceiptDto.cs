using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) for creating a new receipt.
/// </summary>
public class CreateReceiptDto
{
    /// <summary>
    /// Gets or sets the user ID associated with the receipt.
    /// </summary>
    [Required]
    public int UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the list of products included(DTO) in the receipt.
    /// </summary>
    public List<CreateReceiptProductDto> Products { get; set; } = new();
}
