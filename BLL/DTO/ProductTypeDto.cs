namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a product type.
/// </summary>
public class ProductTypeDto : BaseDeletableDto
{
    /// <summary>
    /// Gets or sets the name of the product type.
    /// </summary>
    public string Name { get; set; } = default!;
}
