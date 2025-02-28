using BLL.DTO;
using DAL.Context;
using Domain.Entities;

namespace BLL.Mappers;

public static class ReceiptMapper
{
    // public static ReceiptDto MapToDto(Receipt entity)
    // {
    //     return new ReceiptDto
    //     {
    //         Id = entity.Id,
    //         UserFullName = entity.User != null 
    //             ? $"{entity.User.Firstname} {entity.User.Surname}" 
    //             : "Unknown User",
    //         PaidAmount = entity.PaidAmount,
    //         CreatedAt = entity.CreatedAt,
    //         Products = entity.ReceiptProducts
    //             .Select(rp => new ReceiptProductDto
    //             {
    //                 ProductId = rp.ProductId,
    //                 ProductName = rp.Product.Name,
    //                 Quantity = rp.Quantity,
    //                 UnitPrice = rp.UnitPrice
    //             })
    //             .ToList()
    //     };
    // }
    
    public static ReceiptDto MapToDto(Receipt entity, AppDbContext context = null)
    {
        var dto = new ReceiptDto
        {
            Id = entity.Id,
            UserFullName = UserMapper.MapToDto(entity.User).FullName,
            PaidAmount = entity.PaidAmount,
            CreatedAt = entity.CreatedAt,
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

        // Kontrollime ainult siis, kui context on olemas
        if (context != null)
        {
            dto.IsOpen = !context.MoneyTransactions.Any(mt => mt.ReceiptId == entity.Id);
    
            // Kontrollime, kas eksisteerib return tüüpi transaction selle retsepti jaoks
            dto.IsReturned = dto.IsReturned || context.MoneyTransactions
                .Any(mt => mt.ReceiptId == entity.Id && mt.Type == TransactionType.Return);
        }
        return dto;
    }

    public static Receipt MapToEntity(ReceiptDto dto)
    {
        return new Receipt
        {
            Id = dto.Id,
            PaidAmount = dto.PaidAmount,
            ModifiedAt = DateTime.UtcNow

        };
    }
    
    public static Receipt MapFromCreateDto(CreateReceiptDto dto)
    {
        return new Receipt
        {
            UserId = dto.UserId,
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
    
    private static ReceiptProductDto MapReceiptProductToDto(ReceiptProduct entity)
    {
        return new ReceiptProductDto
        {
            ProductId = entity.ProductId,
            ProductName = entity.Product.Name,
            Quantity = entity.Quantity,
            UnitPrice = entity.UnitPrice
        };
    }
}