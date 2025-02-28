using BLL.DTO;

namespace BLL.Services;

public interface IProductTypeService
{
    Task<List<ProductTypeDto>> GetAllAsync();
    Task<ProductTypeDto> GetByIdAsync(int id);
    Task<ProductTypeDto> CreateAsync(CreateProductTypeDto dto);
    Task<ProductTypeDto> UpdateAsync(int id, CreateProductTypeDto dto);
    Task DeleteAsync(int id);
}