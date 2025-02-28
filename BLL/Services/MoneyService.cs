using BLL.DTO;
using BLL.Exceptions;
using BLL.Mappers;
using DAL.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class MoneyService : IMoneyService
{
    private readonly AppDbContext _context;

    public MoneyService(AppDbContext context)
    {
        _context = context;
    }

    // Get total balance
    public async Task<decimal> GetCurrentBalanceAsync()
    {
        return await _context.MoneyTransactions.SumAsync(t => t.Amount);
    }

    // Get all transactions or transactions made in time range.
    public async Task<List<MoneyTransactionDto>> GetTransactionsAsync(DateTime? from = null, DateTime? to = null)
    {
        var query = _context.MoneyTransactions.AsQueryable();

        if (from.HasValue)
            query = query.Where(t => t.CreatedAt >= from.Value);
    
        if (to.HasValue)
            query = query.Where(t => t.CreatedAt <= to.Value);

        var transactions = await query
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    
        return transactions.Select(MoneyTransactionMapper.MapToDto).ToList();
    }
    
    public async Task<MoneyTransactionDto> GetByIdAsync(int id)
    {
        var transaction = await _context.MoneyTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
        {
            throw new NotFoundException($"Transaction with id {id} not found");
        }

        return MoneyTransactionMapper.MapToDto(transaction);
    }
    
    public async Task<MoneyTransactionDto> AddSaleTransactionAsync(int receiptId, decimal amount)
    {
        var transaction = MoneyTransactionMapper.MapFromSale(receiptId, amount);
        
        _context.MoneyTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return MoneyTransactionMapper.MapToDto(transaction);
    }
    
    public async Task<MoneyTransactionDto> AddReturnTransactionAsync(int receiptId, decimal amount)
    {
        var transaction = MoneyTransactionMapper.MapFromReturn(receiptId, amount);
        
        _context.MoneyTransactions.Add(transaction);
        await _context.SaveChangesAsync();

        return MoneyTransactionMapper.MapToDto(transaction);
    }

    // Daily balance (MB in future need)
    public async Task<decimal> GetDayBalanceAsync(DateTime date)
    {
        return await _context.MoneyTransactions
            .Where(t => t.CreatedAt.Date == date.Date)
            .SumAsync(t => t.Amount);
    }
}