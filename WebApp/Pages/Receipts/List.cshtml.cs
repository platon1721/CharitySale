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
                    Receipts = new List<ReceiptDto>();
                }

                return Page();
            }
            catch (Exception)
            {
                Receipts = new List<ReceiptDto>();
                return Page();
            }
        }
    }
}
