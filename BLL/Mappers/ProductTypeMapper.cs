using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

public class ProductTypeMapper
{
    public static ProductTypeDto MapToDto(ProductType entity)
    {
        return new ProductTypeDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    public static ProductType MapToEntity(ProductTypeDto dto)
    {
        return new ProductType
        {
            Id = dto.Id,
            Name = dto.Name,
            ModifiedAt = DateTime.UtcNow
        };
    }
    
    public static ProductType MapFromCreateDto(CreateProductTypeDto dto)
    {
        return new ProductType
        {
            Name = dto.Name,
            CreatedAt = DateTime.UtcNow,
            ModifiedAt = DateTime.UtcNow
        };
    }
}