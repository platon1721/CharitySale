using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class MoneyTransaction: BaseEntity
{
    
    [Required]
    [Range(-10000, 10000)]
    public decimal Amount { get; set; }
    
    [Required]
    public TransactionType Type { get; set; }
    
    [Required]
    public int ReceiptId { get; set; }
    public virtual Receipt Receipt { get; set; } = default!;
    
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = default!;
}

public enum TransactionType
{
    Sale,
    Return
}