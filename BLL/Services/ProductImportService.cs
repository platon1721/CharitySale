using BLL.DTO;
using DAL.Context;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductImportService : IProductImportService
{
    private readonly AppDbContext _context;
    private readonly IProductConfigurationService _configService;

    public ProductImportService(AppDbContext context, IProductConfigurationService configService)
    {
        _context = context;
        _configService = configService;
    }

    public async Task<ImportResult> ImportProductsFromFileAsync(IFormFile configFile)
    {
        var result = new ImportResult();
        
        try
        {
            var productsToImport = await _configService.ReadProductsFromConfigAsync(configFile);
            result = await ImportProductsAsync(productsToImport);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Failed to import products: {ex.Message}";
        }
        
        return result;
    }

    public async Task<ImportResult> ImportProductsFromDefaultConfigAsync()
    {
        var result = new ImportResult();
        
        try
        {
            var productsToImport = await _configService.ReadProductsFromDefaultConfigAsync();
            result = await ImportProductsAsync(productsToImport);
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"Failed to import products from default config: {ex.Message}";
        }
        
        return result;
    }

    private async Task<ImportResult> ImportProductsAsync(List<ProductImportDto> productsToImport)
    {
        var result = new ImportResult
        {
            TotalItems = productsToImport.Count
        };

        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            foreach (var productDto in productsToImport)
            {
                // Check if product already exists
                var existingProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Name == productDto.Name);

                if (existingProduct != null)
                {
                    // Update existing product
                    existingProduct.Description = productDto.Description;
                    existingProduct.Price = productDto.Price;
                    existingProduct.Stock = productDto.Stock;
                    existingProduct.ProductTypeId = productDto.ProductTypeId;
                    existingProduct.ModifiedAt = DateTime.UtcNow;
                    
                    result.UpdatedItems++;
                }
                else
                {
                    // Check if product type exists
                    var productTypeExists = await _context.ProductTypes
                        .AnyAsync(pt => pt.Id == productDto.ProductTypeId);
                    
                    if (!productTypeExists)
                    {
                        result.SkippedItems++;
                        result.SkippedItemDetails.Add($"Product '{productDto.Name}' skipped: ProductTypeId {productDto.ProductTypeId} does not exist.");
                        continue;
                    }
                    
                    // Add new product
                    var newProduct = new Product
                    {
                        Name = productDto.Name,
                        Description = productDto.Description,
                        Price = productDto.Price,
                        Stock = productDto.Stock,
                        ProductTypeId = productDto.ProductTypeId,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };
                    
                    await _context.Products.AddAsync(newProduct);
                    result.AddedItems++;
                }
            }
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            result.Success = true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            result.Success = false;
            result.ErrorMessage = $"Database error during import: {ex.Message}";
        }
        
        return result;
    }
}