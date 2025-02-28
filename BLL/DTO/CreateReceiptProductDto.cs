namespace BLL.DTO;


/// <summary>
/// Data Transfer Object (DTO) for adding a product to a receipt.
/// </summary>
public class CreateReceiptProductDto
{
    
    /// <summary>
    /// Gets or sets the product ID associated with the receipt product.
    /// </summary>
    public int ProductId { get; set; }
    
    /// <summary>
    /// Gets or sets the receipt ID to which the product belongs.
    /// </summary>
    public int ReceiptId { get; set; }
    
    /// <summary>
    /// Gets or sets the quantity of the product included in the receipt.
    /// </summary>
    public int Quantity { get; set; }
}