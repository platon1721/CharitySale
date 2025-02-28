using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;


namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
[SwaggerTag("Money Transactions")]
public class MoneyTransactionsController: ControllerBase
{
    private readonly IMoneyService _moneyService;

    public MoneyTransactionsController(IMoneyService moneyService)
    {
        _moneyService = moneyService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get Money Transactions", Description = "Get all Money Transactions")]
    public async Task<IActionResult> Get()
    {
        var transactions = await _moneyService.GetTransactionsAsync();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get Money Transaction", Description = "Get Money Transaction by id")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await _moneyService.GetByIdAsync(id);
        return Ok(transaction);
    }

    [HttpGet("Balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get current balance", Description = "Get current full balance")]
    public async Task<IActionResult> GetBalance()
    {
        var bance = _moneyService.GetCurrentBalanceAsync();
        return Ok(bance);
    }

    [HttpGet("Balance/{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get daily balance", Description = "Get balance of the chosen date")]
    public async Task<IActionResult> GetBalance(DateTime date)
    {
        var balance = _moneyService.GetDayBalanceAsync(date);
        return Ok(balance);
    }
    
    [HttpPost("sale")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Add a sale transaction", Description = "Creates a new transaction for a sale")]
    public async Task<ActionResult<MoneyTransactionDto>> AddSaleTransaction(
        [FromBody] SaleReturnTransactionDto dto)
    {
        Console.WriteLine($"Received sale transaction. ReceiptId: {dto.ReceiptId}, Amount: {dto.Amount}");

        var transaction = await _moneyService.AddSaleTransactionAsync(dto.ReceiptId, dto.Amount);
        return Ok(transaction);
    }
    
    [HttpPost("return")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Add a return transaction", Description = "Creates a new transaction for a product return")]
    public async Task<ActionResult<MoneyTransactionDto>> AddReturnTransaction(
        [FromBody] SaleReturnTransactionDto dto)
    {
        var transaction = await _moneyService.AddReturnTransactionAsync(dto.ReceiptId, dto.Amount);
        return Ok(transaction);
    }
}