using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductTypeService: IProductTypeService
{
    
    private readonly AppDbContext _context;

    public ProductTypeService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProductTypeDto>> GetAllAsync()
    {
        var productTypes = await _context.ProductTypes
            .AsNoTracking()
            .ToListAsync();
        
        return productTypes.Select(ProductTypeMapper.MapToDto).ToList();
    }

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

    public async Task<ProductTypeDto> CreateAsync(CreateProductTypeDto dto)
    {
        var productType = ProductTypeMapper.MapFromCreateDto(dto);

        _context.ProductTypes.Add(productType);
        await _context.SaveChangesAsync();

        return ProductTypeMapper.MapToDto(productType);
    }

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