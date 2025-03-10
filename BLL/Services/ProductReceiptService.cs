using System.Data;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductReceiptService : IProductReceiptService
{
    private readonly AppDbContext _context;
    private readonly IMoneyService _moneyService;

    public ProductReceiptService(AppDbContext context, IMoneyService moneyService)
    {
        _context = context;
        _moneyService = moneyService;
    }

    /// <summary>
    /// Updates the quantity of a specific product in a receipt. If the quantity increases, it checks and decreases 
    /// available product stock. If the quantity decreases, it returns stock back to inventory. If quantity is set 
    /// to zero, the product is removed from the receipt entirely.
    /// </summary>
    /// <param name="receiptId">The ID of the receipt to update</param>
    /// <param name="productId">The ID of the product within the receipt to update</param>
    /// <param name="dto">Data transfer object containing the new quantity</param>
    /// <returns>Updated receipt data transfer object with all related information</returns>
    /// <exception cref="NotFoundException">Thrown when the product is not found in the specified receipt</exception>
    /// <exception cref="OutOfStockException">Thrown when trying to increase quantity beyond available stock</exception>
    public async Task<ReceiptDto> UpdateProductQuantityAsync(int receiptId, int productId, UpdateReceiptProductDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var receiptProduct = await _context.ReceiptProducts
                .Include(rp => rp.Receipt)
                .Include(rp => rp.Product)
                .FirstOrDefaultAsync(rp => rp.ReceiptId == receiptId && rp.ProductId == productId);

            if (receiptProduct == null)
                throw new NotFoundException("Product not found in receipt");

            var product = receiptProduct.Product;
            var quantityDifference = dto.Quantity - receiptProduct.Quantity;

            if (quantityDifference > 0)
            {
                if (product.Stock < quantityDifference)
                    throw new OutOfStockException($"Not enough stock. Available: {product.Stock}");
                
                product.Stock -= quantityDifference;
                product.ModifiedAt = DateTime.UtcNow;
            }
            else if (quantityDifference < 0)
            {
                product.Stock += Math.Abs(quantityDifference);
                product.ModifiedAt = DateTime.UtcNow;
            }

            if (dto.Quantity == 0)
            {
                _context.ReceiptProducts.Remove(receiptProduct);
            }
            else
            {
                receiptProduct.Quantity = dto.Quantity;
            }

            receiptProduct.Receipt.PaidAmount += (product.Price * quantityDifference);
            receiptProduct.Receipt.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var updatedReceipt = await _context.Receipts
                .Include(r => r.User)
                .Include(r => r.ReceiptProducts)
                    .ThenInclude(rp => rp.Product)
                .FirstAsync(r => r.Id == receiptId);

            return ReceiptMapper.MapToDto(updatedReceipt);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// Method to remove product from the receipt.
    /// </summary>
    /// <param name="receiptId">Unique identifier of the receipt where from to delete the product</param>
    /// <param name="productId">Unique identifier of the product to be deleted.</param>
    /// <returns>Receipt DTO, without product.</returns>
    /// <exception cref="NotFoundException">
    /// Would be thrown if the product is not in the receipt or receipt is not found</exception>
    public async Task<ReceiptDto> RemoveProductAsync(int receiptId, int productId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var receiptProduct = await _context.ReceiptProducts
                .Include(rp => rp.Receipt)
                .Include(rp => rp.Product)
                .FirstOrDefaultAsync(rp => rp.ReceiptId == receiptId && rp.ProductId == productId);

            if (receiptProduct == null)
                throw new NotFoundException("Product not found in receipt");
            
            if (receiptProduct.Product == null)
                throw new NotFoundException($"Product with ID {productId} is null");
    
            if (receiptProduct.Receipt == null)
                throw new NotFoundException($"Receipt with ID {receiptId} is null");
            
            var product = receiptProduct.Product;
            var receipt = receiptProduct.Receipt;

            // Return product into the stock
            product.Stock += receiptProduct.Quantity;
            product.ModifiedAt = DateTime.UtcNow;

            // Manage quantity of the product from the raceipt
            receipt.PaidAmount -= (product.Price * receiptProduct.Quantity);
            receipt.ModifiedAt = DateTime.UtcNow;

            // delete product fro the receipt
            _context.ReceiptProducts.Remove(receiptProduct);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var updatedReceipt = await _context.Receipts
                .Include(r => r.User)
                .Include(r => r.ReceiptProducts)
                    .ThenInclude(rp => rp.Product)
                .FirstAsync(r => r.Id == receiptId);

            return ReceiptMapper.MapToDto(updatedReceipt, _context);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}