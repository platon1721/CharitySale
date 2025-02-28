using BLL.DTO;

namespace BLL.Services;

public interface IReceiptService
{
    Task<List<ReceiptDto>> GetAllAsync();
    Task<ReceiptDto> GetByIdAsync(int id);
    Task RestoreStockAsync(int id);
    Task<List<ReceiptDto>> GetReceiptsOfUserAsync(int userId);
    Task<ReceiptDto> CreateAsync(CreateReceiptDto dto);
    Task<ReceiptDto> UpdateAsync(int id, CreateReceiptDto dto);
    Task DeleteAsync(int id);

    Task<ReceiptDto> AddProductToReceiptAsync(int receiptId, AddProductToReceiptDto dto);
}