namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a sale return transaction.
/// </summary>
public class SaleReturnTransactionDto
{
    
    /// <summary>
    /// Gets or sets the ID of the receipt associated with the return transaction.
    /// </summary>
    public int ReceiptId { get; set; }
    
    /// <summary>
    /// Gets or sets the amount refunded in the return transaction.
    /// </summary>
    public decimal Amount { get; set; }
}