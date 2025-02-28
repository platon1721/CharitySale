using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

public static class ProductMapper
{
    public static ProductDto MapToDto(Product entity)
    {
        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Description = entity.Description,
            Quantity = entity.Stock,
            ImageUrl = entity.ImageUrl,
            ProductTypeName = entity.ProductType.Name,
            ProductTypeId = entity.ProductTypeId,
            IsInStock = entity.Stock > 0
        };
    }
    
    public static Product MapToEntity(CreateProductDto dto)
    {
        return new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            ProductTypeId = dto.ProductTypeId,
            Description = dto.Description,
            Stock = dto.Quantity,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
    
    public static Product MapToEntity(ProductDto dto)
    {
        return new Product
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description,
            Stock = dto.Quantity,
            ImageUrl = dto.ImageUrl,
            ProductTypeId = dto.ProductTypeId,
            ModifiedAt = DateTime.UtcNow
        };
    }
}