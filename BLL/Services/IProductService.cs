using BLL.DTO;

namespace BLL.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<List<ProductDto>> GetAllActiveAsync();
    Task<ProductDto> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto> UpdateAsync(int id, CreateProductDto dto);
    Task DeleteAsync(int id);
    Task<List<ProductStockDto>> GetProductsStockStatusAsync(List<int> productIds);
    Task<ProductDto> UpdateStockAsync(int id, int quantityChange);
}