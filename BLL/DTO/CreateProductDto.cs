using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) for creating a new product.
/// </summary>
public class CreateProductDto
{
    
    /// <summary>
    /// Gets or sets the product name. Maximum length is 128 characters.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the product price. Must be a non-negative value.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the associated product type ID.
    /// </summary>
    [Required]
    public int ProductTypeId { get; set; }
    
    /// <summary>
    /// Gets or sets the product description. Maximum length is 255 characters.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the available product quantity. Must be a non-negative value.
    /// </summary>
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}