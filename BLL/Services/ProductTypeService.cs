using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;


/// <summary>
/// Service for managing product types, including CRUD operations.
/// </summary>
public class ProductTypeService: IProductTypeService
{
    
    private readonly AppDbContext _context;

    public ProductTypeService(AppDbContext context)
    {
        _context = context;
    }
    
    // Retrieves all product types from the database.
    public async Task<List<ProductTypeDto>> GetAllAsync()
    {
        var productTypes = await _context.ProductTypes
            .AsNoTracking()
            .ToListAsync();
        
        return productTypes.Select(ProductTypeMapper.MapToDto).ToList();
    }

    
    // Retrieves a product type by its identifier.
    public async Task<ProductTypeDto> GetByIdAsync(int id)
    {
        var productType = await _context.ProductTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(pt => pt.Id == id);

        if (productType == null)
        {
            throw new NotFoundException($"ProductType with id {id} was not found");
        }

        return ProductTypeMapper.MapToDto(productType);
    }

    
    // Creates a new product type in the database.
    public async Task<ProductTypeDto> CreateAsync(CreateProductTypeDto dto)
    {
        var productType = ProductTypeMapper.MapFromCreateDto(dto);

        _context.ProductTypes.Add(productType);
        await _context.SaveChangesAsync();

        return ProductTypeMapper.MapToDto(productType);
    }

    // Updates an existing product type in the database.
    public async Task<ProductTypeDto> UpdateAsync(int id, CreateProductTypeDto dto)
    {
        var productType = await _context.ProductTypes.FindAsync(id);

        if (productType == null)
        {
            throw new NotFoundException($"ProductType with id {id} was not found");
        }

        productType.Name = dto.Name;
        productType.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ProductTypeMapper.MapToDto(productType);
    }

    // Deletes a product type from the database.
    // Need to think how to use soft delete here. Mb add basic product type like "other products"
    // or "No category products".
    public async Task DeleteAsync(int id)
    {
        var productType = await _context.ProductTypes.FindAsync(id);

        if (productType == null)
        {
            throw new NotFoundException($"ProductType with id {id} was not found");
        }

        _context.ProductTypes.Remove(productType);
        await _context.SaveChangesAsync();
    }
}