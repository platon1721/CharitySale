using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) representing a money transaction.
/// </summary>
public class MoneyTransactionDto : BaseDeletableDto
{
    /// <summary>
    /// Gets or sets the amount of money involved in the transaction.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Gets or sets the type of the transaction (e.g., income or expense).
    /// </summary>
    public TransactionType Type { get; set; }
    
    /// <summary>
    /// Gets or sets the ID of the associated receipt.
    /// </summary>
    public int ReceiptId { get; set; }
    
    /// <summary>
    /// Gets or sets the description of the transaction.
    /// </summary>
    public string Description { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the date and time when the transaction was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
}