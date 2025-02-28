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
    public class IndexModel : AuthenticatedPageModel
{
    private readonly HttpClient _httpClient;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("WebApi");
    }

    public List<ReceiptDto> UserReceipts { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try 
        {
            var response = await _httpClient.GetAsync($"api/Receipts/user/{UserId}");
    
            if (response.IsSuccessStatusCode)
            {
                UserReceipts = await response.Content.ReadFromJsonAsync<List<ReceiptDto>>() 
                                 ?? new List<ReceiptDto>();
            }
        }
        catch 
        {
            UserReceipts = new List<ReceiptDto>();
        }

        return Page();
    }

    
    public async Task<IActionResult> OnPostCreateNewReceiptAsync()
    {
        if (!UserId.HasValue)
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var createReceiptDto = new CreateReceiptDto
            {
                UserId = UserId.Value
            };
        
            Console.WriteLine($"IndexModel - Sending UserId: {createReceiptDto.UserId}");
        
            var response = await _httpClient.PostAsJsonAsync("api/Receipts", createReceiptDto);
        
            Console.WriteLine($"IndexModel - Response Status: {response.StatusCode}");
        
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"IndexModel - Error response: {response.StatusCode}, Content: {errorContent}");
            }
        
            if (response.IsSuccessStatusCode)
            {
                var receipt = await response.Content.ReadFromJsonAsync<ReceiptDto>();
                Console.WriteLine($"IndexModel - Created receipt ID: {receipt?.Id}");
                return RedirectToPage("./Edit", new { id = receipt.Id });
            }
        
            ModelState.AddModelError(string.Empty, "Failed to create receipt");
            return Page();
        }
        catch (Exception e)
        {
            Console.WriteLine($"IndexModel - Exception details: {e.Message}");
            Console.WriteLine($"Stack trace: {e.StackTrace}");
        
            ModelState.AddModelError(string.Empty, "An error occurred while creating the receipt");
            return Page();
        }
    }
}
}
