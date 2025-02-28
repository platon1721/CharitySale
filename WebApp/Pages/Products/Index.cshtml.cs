using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL.Context;
using Domain.Entities;
using WebApp.ApiControllers;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Products
{
    public class IndexModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(
            IHttpClientFactory httpClientFactory, 
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ProductDto> Products { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Check authorisation if needed
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Uncomment the following line if you want to redirect unauthorized users
                // return RedirectToPage("/Users/Login");
            }

            try 
            {
                var response = await _httpClient.GetAsync("api/Products");
            
                if (response.IsSuccessStatusCode)
                {
                    Products = await response.Content.ReadFromJsonAsync<List<ProductDto>>() 
                               ?? new List<ProductDto>();
                }
            }
            catch 
            {
                Products = new List<ProductDto>();
                // Optionally, you can add an error message to ModelState here
                // ModelState.AddModelError(string.Empty, "An error occurred while fetching products.");
            }

            return Page();
        }
    }
}
