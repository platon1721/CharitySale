using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace BLL.DTO;

public class MoneyTransactionDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public int ReceiptId { get; set; }
    public string Description { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    
}