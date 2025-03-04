using BLL.DTO;
using DAL.Context;
using Domain.Entities;

namespace BLL.Mappers;

/// <summary>
/// Provides mapping methods for converting between Receipt entities and DTOs.
/// </summary>
public static class ReceiptMapper
{
    
    
    /// <summary>
    /// Maps a Receipt entity to a ReceiptDto.
    /// </summary>
    /// <param name="entity">The receipt entity to map.</param>
    /// <returns>A DTO representing the receipt.</returns>
    public static ReceiptDto MapToDto(Receipt entity)
    { 
        var dto = new ReceiptDto
        {
            Id = entity.Id,
            UserFullName = UserMapper.MapToDto(entity.User).FullName,
            PaidAmount = entity.PaidAmount,
            CreatedAt = entity.CreatedAt,
            IsOpen = entity.IsOpen,
            IsReturned = entity.IsDeleted,
            ReturnedAt = entity.DeletedAt,
            Products = entity.ReceiptProducts
                .Where(rp => rp.Product != null)
                .Select(rp => new ReceiptProductDto
                {
                    ProductId = rp.ProductId,
                    ProductName = rp.Product.Name,
                    Quantity = rp.Quantity,
                    UnitPrice = rp.UnitPrice
                })
                .ToList()
        };
        return dto;
    }

    /// <summary>
    /// Maps a ReceiptDto to a Receipt entity.
    /// </summary>
    /// <param name="dto">The DTO containing receipt data.</param>
    /// <returns>A receipt entity.</returns>
    public static Receipt MapToEntity(ReceiptDto dto)
    {
        return new Receipt
        {
            Id = dto.Id,
            PaidAmount = dto.PaidAmount,
            ModifiedAt = DateTime.UtcNow,
            IsOpen = dto.IsOpen,
            IsDeleted = dto.IsReturned,
            DeletedAt = dto.ReturnedAt
        };
    }
    
    
    /// <summary>
    /// Maps a CreateReceiptDto to a new Receipt entity.
    /// </summary>
    /// <param name="dto">The DTO containing receipt creation data.</param>
    /// <returns>A new receipt entity.</returns>
    public static Receipt MapFromCreateDto(CreateReceiptDto dto, User user)
    {
        return new Receipt
        {
            UserId = dto.UserId,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow,
            ReceiptProducts = dto.Products
                .Select(p => new ReceiptProduct
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = 0
                })
                .ToList()
        };
    }
}