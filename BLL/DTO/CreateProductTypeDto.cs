using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) for creating a new product type.
/// </summary>
public class CreateProductTypeDto
{
    
    /// <summary>
    /// Gets or sets the product type name. Maximum length is 50 characters.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = default!;
}