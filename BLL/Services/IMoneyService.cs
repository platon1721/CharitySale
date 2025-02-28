using BLL.DTO;

namespace BLL.Services;

public interface IMoneyService
{
    Task<decimal> GetCurrentBalanceAsync();
    Task<List<MoneyTransactionDto>> GetTransactionsAsync(DateTime? from = null, DateTime? to = null);
   
    Task<MoneyTransactionDto> GetByIdAsync(int id);
    Task<MoneyTransactionDto> AddSaleTransactionAsync(int receiptId, decimal amount);
    Task<MoneyTransactionDto> AddReturnTransactionAsync(int receiptId, decimal amount);
    Task<decimal> GetDayBalanceAsync(DateTime date);
}