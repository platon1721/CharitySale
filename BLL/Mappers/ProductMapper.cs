using BLL.DTO;
using Domain.Entities;

namespace BLL.Mappers;

/// <summary>
/// Provides mapping methods for converting between Product entities and ProductDto DTOs.
/// </summary>
public static class ProductMapper
{
    
    ///  /// <summary>
    /// Maps a Product entity to a ProductDto>.
    /// </summary>
    /// <param name="entity">The product entity to map.</param>
    /// <returns>A DTO representing the product.</returns>
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
            IsInStock = entity.Stock > 0,
            IsDeleted = entity.IsDeleted,
            DeletedAt = entity.DeletedAt
        };
    }
    
    
    /// /// <summary>
    /// Maps a CreateProductDto to a Product entity.
    /// </summary>
    /// <param name="dto">The DTO containing product creation data.</param>
    /// <returns>A new product entity.</returns>
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
    
    /// /// <summary>
    /// Maps a ProductDto to a Product entity.
    /// </summary>
    /// <param name="dto">The DTO containing product data.</param>
    /// <returns>A product entity.</returns>
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