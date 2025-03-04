using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;


/// <summary>
/// Provides mapping methods for converting between MoneyTransaction entities and MoneyTransactionDto DTOs.
/// </summary>
public class MoneyTransactionMapper
{
    
    /// <summary>
    /// Maps a MoneyTransaction entity to a MoneyTransactionDto.
    /// </summary>
    /// <param name="entity">The money transaction entity to map.</param>
    /// <returns>A DTO representing the money transaction.</returns>
    public static MoneyTransactionDto MapToDto(MoneyTransaction entity)
    {
        return new MoneyTransactionDto
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Description = entity.Description,
            Type = entity.Type,
            ReceiptId = entity.ReceiptId,
            CreatedAt = entity.CreatedAt,
            IsDeleted = entity.IsDeleted,
            DeletedAt = entity.DeletedAt,
        };
    }
    
    
    /// <summary>
    /// Creates a new MoneyTransaction entity for a sale transaction.
    /// </summary>
    /// <param name="receiptId">The ID of the receipt associated with the transaction.</param>
    /// <param name="amount">The amount of the sale.</param>
    /// <returns>A new money transaction entity representing the sale.</returns>
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
    
    /// <summary>
    /// Creates a new MoneyTransaction entity for a return transaction.
    /// </summary>
    /// <param name="receiptId">The ID of the receipt associated with the return transaction.</param>
    /// <param name="amount">The amount to be refunded.</param>
    /// <returns>A new money transaction entity representing the return.</returns>
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