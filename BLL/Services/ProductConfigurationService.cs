using System.Text.Json;
using BLL.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;


/// <summary>
/// Service for managing product configurations.
/// </summary>
public class ProductConfigurationService : IProductConfigurationService
{
    private readonly IWebHostEnvironment _environment;

    public ProductConfigurationService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    
    // Reads products from a provided configuration file.
    public async Task<List<ProductImportDto>> ReadProductsFromConfigAsync(IFormFile configFile)
    {
        try
        {
            using var stream = configFile.OpenReadStream();
            using var reader = new StreamReader(stream);
            var jsonContent = await reader.ReadToEndAsync();
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var productsData = JsonSerializer.Deserialize<ProductsConfigData>(jsonContent, options);
            
            return productsData?.Products?.ToList() ?? new List<ProductImportDto>();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error parsing products configuration file: {ex.Message}", ex);
        }
    }

    
    // Reads products from the default configuration file.
    public async Task<List<ProductImportDto>> ReadProductsFromDefaultConfigAsync()
    {
        var configPath = Path.Combine(_environment.ContentRootPath, "Config", "products.json");
        
        if (!File.Exists(configPath))
        {
            throw new FileNotFoundException("Default products configuration file not found");
        }

        try
        {
            var jsonContent = await File.ReadAllTextAsync(configPath);
            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var productsData = JsonSerializer.Deserialize<ProductsConfigData>(jsonContent, options);
            
            return productsData?.Products?.ToList() ?? new List<ProductImportDto>();
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error reading default products configuration: {ex.Message}", ex);
        }
    }
}