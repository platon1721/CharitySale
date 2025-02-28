using System.Data;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.Services;

/// <summary>
/// Service for managing receipts, including CRUD operations and product management within receipts.
/// </summary>
public class ReceiptService: IReceiptService
{
    
    private readonly AppDbContext _context;
    private readonly IMoneyService _moneyService;

    public ReceiptService(AppDbContext context, IMoneyService moneyService)
    {
        _context = context;
        _moneyService = moneyService;
    }
    
    
    // Retrieves all receipts from the database.
    public async Task<List<ReceiptDto>> GetAllAsync()
    {
        var receipts = await _context.Receipts
            .Include(r => r.User) 
            .Include(r => r.ReceiptProducts)
            .ThenInclude(rp => rp.Product)
            .AsNoTracking()
            .ToListAsync();

        return receipts.Select(receipt => ReceiptMapper.MapToDto(receipt, _context)).ToList();
    }

    // Retrieves a receipt by its unique identifier.
    public async Task<ReceiptDto> GetByIdAsync(int id)
    {
        var receipt = await _context.Receipts
            .Include(r => r.User)
            .Include(r => r.ReceiptProducts)
            .ThenInclude(rp => rp.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(receipt => receipt.Id == id);
        if (receipt == null)
        {
            throw new NotFoundException($"Receipt with id={id} not found");
        }

        return ReceiptMapper.MapToDto(receipt, _context);
    }

    
    // Creates a new receipt in the database.
    public async Task<ReceiptDto> CreateAsync(CreateReceiptDto dto)
    {
        var receipt = ReceiptMapper.MapFromCreateDto(dto);
        var existingUser = await _context.Users.FindAsync(dto.UserId);
        if (existingUser == null)
        {
            throw new Exception($"User with ID {dto.UserId} not found");
        }

        await _context.Receipts.AddAsync(receipt);
        await _context.SaveChangesAsync();

        return ReceiptMapper.MapToDto(receipt);
    }
    
    // Retrieves all receipts for a specific user.
    public async Task<List<ReceiptDto>> GetReceiptsOfUserAsync(int userId)
    {
        var userReceipts = await _context.Receipts
            .Include(r => r.User)
            .Include(r => r.ReceiptProducts)
            .ThenInclude(rp => rp.Product)
            .Where(r => r.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

        return userReceipts.Select(receipt => ReceiptMapper.MapToDto(receipt, _context)).ToList();
    }

    
    
    public Task<ReceiptDto> UpdateAsync(int id, CreateReceiptDto dto)
    {
        throw new NotImplementedException();
    }

    
    // Deletes a receipt from the database and restores products in stock.
    public async Task DeleteAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var receipt = await _context.Receipts
                .Include(r => r.ReceiptProducts)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (receipt == null)
            {
                throw new NotFoundException($"Receipt with id {id} not found");
            }
            
            await _moneyService.AddReturnTransactionAsync(
                receipt.Id,
                receipt.PaidAmount
            );

            // Update in stock amounts of products
            foreach (var receiptProduct in receipt.ReceiptProducts)
            {
                var product = await _context.Products.FindAsync(receiptProduct.ProductId);
                if (product != null)
                {
                    product.Stock += receiptProduct.Quantity;
                    product.ModifiedAt = DateTime.UtcNow;
                }
            }

            // Delete the receipt
            _context.Receipts.Remove(receipt);
            await _context.SaveChangesAsync();
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    
    // Method that is used for closed tabs.
    // To hold receipt in the DB, when the return is made.
    // Returns all products back into the stock.
    public async Task RestoreStockAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var receipt = await _context.Receipts
                .Include(r => r.ReceiptProducts)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (receipt == null)
            {
                throw new NotFoundException($"Receipt with id {id} not found");
            }
            
            receipt.ModifiedAt = DateTime.UtcNow;

            // Update in stock amounts of products
            foreach (var receiptProduct in receipt.ReceiptProducts)
            {
                var product = await _context.Products.FindAsync(receiptProduct.ProductId);
                if (product != null)
                {
                    product.Stock += receiptProduct.Quantity;
                    product.ModifiedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    
    // Task to add product into the receipt.
    // If Product is already in receipt just manipulates with its quantity, else creates new Receipt-Product.
    // Changes in stock quantity and final price of the bill.
    public async Task<ReceiptDto> AddProductToReceiptAsync(int receiptId, AddProductToReceiptDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            // Find the receipt
            var receipt = await _context.Receipts
                .Include(r => r.ReceiptProducts)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == receiptId);
    
            if (receipt == null)
                throw new NotFoundException($"Receipt with id {receiptId} not found");
    
            // Find product
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);
    
            if (product == null)
                throw new NotFoundException($"Product with id {dto.ProductId} not found");
    
            if (product.Stock < dto.Quantity)
                throw new OutOfStockException($"Not enough stock for product {product.Name}. Available: {product.Stock}");
    
            // Check if product is already in this receipt
            var existingProduct = receipt.ReceiptProducts
                .FirstOrDefault(rp => rp.ProductId == dto.ProductId);
    
            if (existingProduct != null)
            {
                // Change quantity
                existingProduct.Quantity += dto.Quantity;
            }
            else
            {
                // Add new product
                var p =
                    new ReceiptProduct
                    {
                        ReceiptId = receiptId,
                        ProductId = dto.ProductId,
                        Quantity = dto.Quantity,
                        UnitPrice = product.Price
                    };
                receipt.ReceiptProducts.Add(p);
            }
    
            // Change product in stock quantity
            product.Stock -= dto.Quantity;
            product.ModifiedAt = DateTime.UtcNow;
    
            // Change final price
            receipt.PaidAmount += (product.Price * dto.Quantity);
            receipt.ModifiedAt = DateTime.UtcNow;
    
            await _context.SaveChangesAsync();
    
            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction.GetDbTransaction().Connection != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
        
        var updatedReceipt = await _context.Receipts
            .Include(r => r.User)
            .Include(r => r.ReceiptProducts)
            .ThenInclude(rp => rp.Product)
            .FirstAsync(r => r.Id == receiptId);
    
        return ReceiptMapper.MapToDto(updatedReceipt, _context);
    }
}