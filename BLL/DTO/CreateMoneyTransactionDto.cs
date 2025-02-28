using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BLL.DTO;

public class CreateMoneyTransactionDto
{
    [Required]
    [Range(-10000, 10000)]
    public decimal Amount { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = default!;
    
    [Required]
    public TransactionType Type { get; set; }
    
    public int ReceiptId { get; set; }
}