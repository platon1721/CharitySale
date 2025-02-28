using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

public class MoneyTransactionMapper
{
    public static MoneyTransactionDto MapToDto(MoneyTransaction entity)
    {
        return new MoneyTransactionDto
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Description = entity.Description,
            Type = entity.Type,
            ReceiptId = entity.ReceiptId,
            CreatedAt = entity.CreatedAt
        };
    }
    
    public static MoneyTransaction MapFromSale(int receiptId, decimal amount)
    {
        return new MoneyTransaction
        {
            Amount = amount,
            Description = $"Sale: Receipt #{receiptId}",
            Type = TransactionType.Sale,
            ReceiptId = receiptId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
    
    public static MoneyTransaction MapFromReturn(int receiptId, decimal amount)
    {
        return new MoneyTransaction
        {
            Amount = -amount,
            Description = $"Return: Receipt #{receiptId}",
            Type = TransactionType.Return,
            ReceiptId = receiptId,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
}