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

    public async Task<ReceiptDto> RemoveProductAsync(int receiptId, int productId)
    {
        Console.WriteLine(receiptId); //14
        Console.WriteLine(productId); //0
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

            // Tagasta toote kogus lattu
            product.Stock += receiptProduct.Quantity;
            product.ModifiedAt = DateTime.UtcNow;

            // Lahuta toote hind retsepti kogusummast
            receipt.PaidAmount -= (product.Price * receiptProduct.Quantity);
            receipt.ModifiedAt = DateTime.UtcNow;

            // Eemalda toode retseptist
            _context.ReceiptProducts.Remove(receiptProduct);

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
}