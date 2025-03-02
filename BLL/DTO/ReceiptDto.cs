namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a receipt.
/// </summary>
public class ReceiptDto : BaseDto
{
    
    /// <summary>
    /// Gets or sets the full name of the user associated with the receipt.
    /// </summary>
    public string UserFullName { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the total amount paid for the receipt.
    /// </summary>
    public decimal PaidAmount { get; set; }
    
    /// <summary>
    /// Gets or sets the list of products included in the receipt.
    /// </summary>
    public List<ReceiptProductDto> Products { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the date and time when the receipt was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the receipt is currently open.
    /// </summary>
    public bool IsOpen { get; set; } = true;
    
    /// <summary>
    /// Gets or sets a value indicating whether the receipt has been returned.
    /// </summary>
    public bool IsReturned { get; set; } = false;
    
    public DateTime? ReturnedAt { get; set; }
}
