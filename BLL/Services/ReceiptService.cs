using System.Data;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.Services;

public class ReceiptService: IReceiptService
{
    
    private readonly AppDbContext _context;
    private readonly IMoneyService _moneyService;

    public ReceiptService(AppDbContext context, IMoneyService moneyService)
    {
        _context = context;
        _moneyService = moneyService;
    }
    // public async Task<List<ReceiptDto>> GetAllAsync()
    // {
    //     var receipts = await _context.Receipts
    //         .Include(r => r.User) 
    //         .Include(r => r.ReceiptProducts)
    //         .ThenInclude(rp => rp.Product)
    //         .AsNoTracking()
    //         .ToListAsync();
    //
    //     return receipts.Select(ReceiptMapper.MapToDto).ToList();
    // }
    
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

    public async Task<ReceiptDto> CreateAsync(CreateReceiptDto dto)
    {
        try 
        {
            Console.WriteLine($"ReceiptService - CreateAsync called with UserId: {dto.UserId}");

            var receipt = ReceiptMapper.MapFromCreateDto(dto);
        
            Console.WriteLine($"ReceiptService - Mapped Receipt - UserId: {receipt.UserId}, CreatedAt: {receipt.CreatedAt}");

            // Lisage andmebaasi kontrollimine
            var existingUser = await _context.Users.FindAsync(dto.UserId);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {dto.UserId} not found");
            }

            await _context.Receipts.AddAsync(receipt);
            await _context.SaveChangesAsync();

            Console.WriteLine($"ReceiptService - Receipt saved with ID: {receipt.Id}");

            return ReceiptMapper.MapToDto(receipt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ReceiptService - Error in CreateAsync: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }
    
    // /// <summary>
    // /// Service to return only open receipts of the current user.
    // /// Open receipt is a receipt that has no yet any money transaction done.
    // /// </summary>
    // /// <param name="userId">Unique identifier of the user.</param>
    // /// <returns>List of ReceiptDto that are not yet completed and are opened by this user.</returns>
    // public async Task<List<ReceiptDto>> GetOpenReceiptsAsync(int userId)
    // {
    //     var openReceipts = await _context.Receipts
    //         .Include(r => r.User)
    //         .Include(r => r.ReceiptProducts)
    //         .ThenInclude(rp => rp.Product)
    //         .Where(r => r.UserId == userId && 
    //                     !_context.MoneyTransactions.Any(mt => mt.ReceiptId == r.Id))
    //         .AsNoTracking()
    //         .ToListAsync();
    //
    //     return openReceipts.Select(ReceiptMapper.MapToDto).ToList();
    // }
    
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

    // public async Task<ReceiptDto> CreateAsync(CreateReceiptDto dto)
    // {
    //     
    //     using var transaction = await _context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         var receipt = ReceiptMapper.MapFromCreateDto(dto);
    //
    //         foreach (var receiptProduct in receipt.ReceiptProducts)
    //         {
    //             var product = await _context.Products.FindAsync(receiptProduct.ProductId);
    //
    //             if (product == null)
    //             {
    //                 throw new NotFoundException($"Product with id {receiptProduct.ProductId} not found");
    //             }
    //
    //             if (product.Stock < receiptProduct.Quantity)
    //             {
    //                 throw new OutOfStockException(
    //                     $"Not enough stock for product {product.Name}. Available: {product.Stock}");
    //             }
    //
    //             receiptProduct.UnitPrice = product.Price;
    //
    //             product.Stock -= receiptProduct.Quantity;
    //             product.ModifiedAt = DateTime.UtcNow;
    //         }
    //
    //         _context.Receipts.Add(receipt);
    //         await _context.SaveChangesAsync();
    //
    //         await _moneyService.AddSaleTransactionAsync(
    //             receipt.Id,
    //             receipt.PaidAmount
    //         );
    //
    //         var createdReceipt = await _context.Receipts
    //             .Include(r => r.User)
    //             .Include(r => r.ReceiptProducts)
    //             .ThenInclude(rp => rp.Product)
    //             .FirstAsync(r => r.Id == receipt.Id);
    //
    //         await transaction.CommitAsync();
    //         return ReceiptMapper.MapToDto(createdReceipt);
    //     }
    //     catch
    //     {
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }
    
    // // Method for global changes in receipt.
    // public async Task<ReceiptDto> UpdateAsync(int id, CreateReceiptDto dto)
    // {
    //     using var transaction = await _context.Database.BeginTransactionAsync();
    //     try
    //     {
    //         // Find receipt with its products
    //         var receipt = await _context.Receipts
    //             .Include(r => r.ReceiptProducts)
    //             .FirstOrDefaultAsync(r => r.Id == id);
    //
    //         if (receipt == null)
    //         {
    //             throw new NotFoundException($"Receipt with id {id} not found");
    //         }
    //         
    //         // Restore in stock amount for all products
    //         foreach (var oldReceiptProduct in receipt.ReceiptProducts)
    //         {
    //             var product = await _context.Products.FindAsync(oldReceiptProduct.ProductId);
    //             if (product != null)
    //             {
    //                 product.Stock += oldReceiptProduct.Quantity;
    //                 product.ModifiedAt = DateTime.UtcNow;
    //             }
    //         }
    //         
    //         // Clear old receipt products
    //         receipt.ReceiptProducts.Clear();
    //         
    //         // Update receipt properties
    //         receipt.UserId = dto.UserId;
    //         receipt.PaidAmount = dto.PaidAmount;
    //         receipt.ModifiedAt = DateTime.UtcNow;
    //
    //         // Add products into new receipt
    //         foreach (var newReceiptProduct in dto.Products)
    //         {
    //             var product = await _context.Products.FindAsync(newReceiptProduct.ProductId);
    //
    //             if (product == null)
    //             {
    //                 throw new NotFoundException($"Product with id {newReceiptProduct.ProductId} not found");
    //             }
    //
    //             if (product.Stock < newReceiptProduct.Quantity)
    //             {
    //                 throw new OutOfStockException(
    //                     $"Not enough stock for product {product.Name}. Available: {product.Stock}");
    //             }
    //
    //             // Create new receipt product
    //             receipt.ReceiptProducts.Add(new ReceiptProduct
    //             {
    //                 ReceiptId = receipt.Id,
    //                 ProductId = newReceiptProduct.ProductId,
    //                 Quantity = newReceiptProduct.Quantity,
    //                 UnitPrice = product.Price
    //             });
    //
    //             // Update in stock amounts
    //             product.Stock -= newReceiptProduct.Quantity;
    //             product.ModifiedAt = DateTime.UtcNow;
    //         }
    //         await _context.SaveChangesAsync();
    //         
    //         // Add new receipt
    //         await _moneyService.AddSaleTransactionAsync(
    //             receipt.Id,
    //             receipt.PaidAmount
    //         );
    //         
    //         var updatedReceipt = await _context.Receipts
    //             .Include(r => r.User)
    //             .Include(r => r.ReceiptProducts)
    //             .ThenInclude(rp => rp.Product)
    //             .FirstAsync(r => r.Id == receipt.Id);
    //         
    //         await transaction.CommitAsync();
    //         return ReceiptMapper.MapToDto(updatedReceipt);
    //     }
    //     catch
    //     {
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }
    

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
                Console.WriteLine($"ProductId here: {p.ProductId}");
                Console.WriteLine($"ProductID of the first PR: {receipt.ReceiptProducts.First().ProductId})");
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

    
    // // Task to update product quantity in receipt.
    // public async Task<ReceiptDto> UpdateProductQuantityAsync(int receiptId, int productId, UpdateReceiptProductQuantityDto dto)
    // {
    //     using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
    //     try
    //     {
    //         // Find product in the receipt.
    //         var receiptProduct = await _context.ReceiptProducts
    //             .Include(rp => rp.Receipt)
    //             .Include(rp => rp.Product)
    //             .FirstOrDefaultAsync(rp => rp.ReceiptId == receiptId && rp.ProductId == productId);
    //
    //         if (receiptProduct == null)
    //             throw new NotFoundException("Product not found in receipt");
    //
    //         // Find product from list of products.
    //         var product = await _context.Products
    //             .FromSqlRaw("SELECT * FROM Products WITH (XLOCK, ROWLOCK) WHERE Id = {0}", productId)
    //             .FirstOrDefaultAsync();
    //         
    //         if (product == null)
    //             throw new NotFoundException($"Product with id {productId} not found");
    //         
    //         var quantityDifference = dto.Quantity - receiptProduct.Quantity;
    //
    //         if (quantityDifference > 0)
    //         {
    //             if (product.Stock < quantityDifference)
    //                 throw new OutOfStockException($"Not enough stock. Available: {product.Stock}");
    //             
    //             // Update in stock quantity (take from stock)
    //             product.Stock -= quantityDifference;
    //             product.ModifiedAt = DateTime.UtcNow;
    //         }
    //         else if (quantityDifference < 0)
    //         {
    //             // Update in stock quantity (put into the stock)
    //             product.Stock += Math.Abs(quantityDifference);
    //             product.ModifiedAt = DateTime.UtcNow;
    //         }
    //
    //         if (dto.Quantity == 0)
    //         {
    //             // If quantity of the product in the receipt is 0, then delete it.
    //             _context.ReceiptProducts.Remove(receiptProduct);
    //         }
    //         else
    //         {
    //             // Update quantity
    //             receiptProduct.Quantity = dto.Quantity;
    //         }
    //
    //         // Update final price of the bill.
    //         receiptProduct.Receipt.PaidAmount += (product.Price * quantityDifference);
    //         receiptProduct.Receipt.ModifiedAt = DateTime.UtcNow;
    //
    //         await _context.SaveChangesAsync();
    //         await transaction.CommitAsync();
    //
    //         var updatedReceipt = await _context.Receipts
    //             .Include(r => r.User)
    //             .Include(r => r.ReceiptProducts)
    //                 .ThenInclude(rp => rp.Product)
    //             .FirstAsync(r => r.Id == receiptId);
    //
    //         return ReceiptMapper.MapToDto(updatedReceipt, _context);
    //     }
    //     catch
    //     {
    //         await transaction.RollbackAsync();
    //         throw;
    //     }
    // }
}