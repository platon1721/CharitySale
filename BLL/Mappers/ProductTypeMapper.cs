using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;


/// <summary>
/// Provides mapping methods for converting between ProductType entities and DTOs.
/// </summary>
public class ProductTypeMapper
{
    
    /// <summary>
    /// Maps a ProductType entity to a ProductTypeDto>.
    /// </summary>
    /// <param name="entity">The product type entity to map.</param>
    /// <returns>A DTO representing the product type.</returns>
    public static ProductTypeDto MapToDto(ProductType entity)
    {
        return new ProductTypeDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    /// <summary>
    /// Maps a ProductTypeDto to a ProductType entity.
    /// </summary>
    /// <param name="dto">The DTO containing product type data.</param>
    /// <returns>A product type entity.</returns>
    public static ProductType MapToEntity(ProductTypeDto dto)
    {
        return new ProductType
        {
            Id = dto.Id,
            Name = dto.Name,
            ModifiedAt = DateTime.UtcNow
        };
    }
    
    /// <summary>
    /// Maps a CreateProductTypeDto to a new ProductType entity.
    /// </summary>
    /// <param name="dto">The DTO containing product type creation data.</param>
    /// <returns>A new product type entity.</returns>
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