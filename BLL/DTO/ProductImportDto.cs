namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) for importing product information.
/// </summary>
public class ProductImportDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the product.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Gets or sets the stock quantity of the product.
    /// </summary>
    public int Stock { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the associated product type.
    /// </summary>
    public int ProductTypeId { get; set; }
}