using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Receipts
{
    public class DetailsModel : AuthenticatedPageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [BindProperty]
        public ReceiptDto Receipt { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try 
            {
                var response = await _httpClient.GetAsync($"api/Receipts/{id}");
            
                if (response.IsSuccessStatusCode)
                {
                    Receipt = await response.Content.ReadFromJsonAsync<ReceiptDto>();
                    return Page();
                }
            
                return NotFound();
            }
            catch 
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostReturnReceiptAsync(int id)
        {
            try
            {
                var transaction = new SaleReturnTransactionDto
                {
                    ReceiptId = id,
                    Amount = Receipt.PaidAmount
                };
        
                var responseTransaction = await _httpClient.PostAsJsonAsync($"api/MoneyTransactions/return", transaction);
                var responseReceipt = await _httpClient.PostAsync($"api/Receipts/{id}/restore-stock", null);

                if (responseTransaction.IsSuccessStatusCode && responseReceipt.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Receipts/Index");
                }

                var errorContentTransaction = await responseTransaction.Content.ReadAsStringAsync();
                Console.WriteLine($"Error returning receipt: {errorContentTransaction}");
                
                var errorContentReceipt = await responseReceipt.Content.ReadAsStringAsync();
                Console.WriteLine($"Error returning receipt: {errorContentReceipt}");

                ModelState.AddModelError(string.Empty, "Failed to return receipt");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ModelState.AddModelError(string.Empty, "An error occurred while returning the receipt");
                return Page();
            }
        }
    }
}
