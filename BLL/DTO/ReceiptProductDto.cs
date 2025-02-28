namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a product included in a receipt.
/// </summary>
public class ReceiptProductDto
{
    /// <summary>
    /// Gets or sets the ID of the product.
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the quantity of the product included in the receipt.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// Gets the total price for the quantity of the product (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
}