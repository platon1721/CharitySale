using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Context;
using Domain.Entities;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Receipts
{
    public class ListModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public ListModel(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public List<ReceiptDto> Receipts { get; set; } = default!;

        
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/receipts");

                if (response.IsSuccessStatusCode)
                {
                    Receipts = await response.Content.ReadFromJsonAsync<List<ReceiptDto>>()
                               ?? new List<ReceiptDto>();
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Receipts = new List<ReceiptDto>();
                }

                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Receipts = new List<ReceiptDto>();
                return Page();
            }
        }
        // public async Task<IActionResult> OnGetAsync()
        // {
        //     var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
        //     if (!userId.HasValue)
        //     {
        //         return RedirectToPage("/Users/Login");
        //     }
        //
        //     try
        //     {
        //         var newReceiptDto = new CreateReceiptDto
        //         {
        //             UserId = userId.Value,
        //             Products = new List<CreateReceiptProductDto>()
        //         };
        //
        //         var response = await _httpClient.PostAsJsonAsync("api/Receipts", newReceiptDto);
        //     
        //         if (response.IsSuccessStatusCode)
        //         {
        //             var createdReceipt = await response.Content.ReadFromJsonAsync<ReceiptDto>();
        //             return RedirectToPage("./Edit", new { id = createdReceipt.Id });
        //         }
        //         else
        //         {
        //             // Handle error
        //             return RedirectToPage("/Error");
        //         }
        //     }
        //     catch
        //     {
        //         return RedirectToPage("/Error");
        //     }
        // }
    }
}
