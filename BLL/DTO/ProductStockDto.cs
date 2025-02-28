namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing the stock information of a product.
/// </summary>
public class ProductStockDto : BaseDto
{
    /// <summary>
    /// Gets or sets the quantity of the product in stock.
    /// </summary>
    public int Stock { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the product is in stock.
    /// </summary>
    public bool IsInStock { get; set; }
}