using BLL.DTO;

namespace BLL.Services;

public interface IProductReceiptService
{
    Task<ReceiptDto> UpdateProductQuantityAsync(int receiptId, int productId, UpdateReceiptProductDto dto);
    Task<ReceiptDto> RemoveProductAsync(int receiptId, int productId);
}