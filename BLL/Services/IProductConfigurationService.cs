using BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public interface IProductConfigurationService
{
    Task<List<ProductImportDto>> ReadProductsFromConfigAsync(IFormFile configFile);
    Task<List<ProductImportDto>> ReadProductsFromDefaultConfigAsync();
}

public class ProductsConfigData
{
    public List<ProductImportDto> Products { get; set; } = new();
}