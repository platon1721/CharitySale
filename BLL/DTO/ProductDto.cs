namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a product.
/// </summary>
public class ProductDto : BaseDeletableDto
{
    
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the available quantity of the product.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the URL of the product image.
    /// </summary>
    public string? ImageUrl { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the product type.
    /// </summary>
    public string ProductTypeName { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets a value indicating whether the product is in stock.
    /// </summary>
    public bool IsInStock { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the associated product type.
    /// </summary>
    public int ProductTypeId { get; set; }
}