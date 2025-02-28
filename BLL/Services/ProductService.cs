using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductService: IProductService
{
    
    private readonly AppDbContext _context;
    
    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Include(p => p.ProductType)
            .AsNoTracking()
            .ToListAsync();
        return products.Select(ProductMapper.MapToDto).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.ProductType)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found!");
        }
        
        return ProductMapper.MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = ProductMapper.MapToEntity(dto);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        
        var createdProduct = await _context.Products
            .Include(p => p.ProductType)
            .FirstAsync(p => p.Id == product.Id);
        
        return ProductMapper.MapToDto(createdProduct);
    }

    public async Task<ProductDto> UpdateAsync(int id, CreateProductDto dto)
    {
        var product = await _context.Products
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found");
        }
        
        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Description = dto.Description;
        product.ProductTypeId = dto.ProductTypeId;
        product.Description = dto.Description;
        product.Stock = dto.Quantity;
        product.ModifiedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return ProductMapper.MapToDto(product);
    }
    
    public async Task<ProductDto> UpdateStockAsync(int id, int quantityChange)
    {
        var product = await _context.Products
            .Include(p => p.ProductType)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found");
        }
        
        if (product.Stock < -quantityChange) 
        {
            throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Current stock: {product.Stock}, Requested change: {-quantityChange}");
        }

        product.Stock += quantityChange;
        product.ModifiedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return ProductMapper.MapToDto(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            throw new NotFoundException($"Product with id {id} was not found");
        }
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<ProductStockDto>> GetProductsStockStatusAsync(List<int> productIds)
    {
        var products = await _context.Products
            .Where(p => productIds.Contains(p.Id))
            .Select(p => new ProductStockDto
            {
                Id = p.Id,
                Stock = p.Stock,
                IsInStock = p.Stock > 0
            })
            .ToListAsync();

        return products;
    }
}