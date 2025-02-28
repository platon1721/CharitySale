using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Context;
using Domain.Entities;
using WebApp.ApiControllers;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Products
{
    public class CreateModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateModel(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public CreateProductDto Product { get; set; } = default!;

        public SelectList ProductTypes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadProductTypes();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadProductTypes();
                return Page();
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Products", Product);
                
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Failed to create product. Server returned: {response.StatusCode} - {errorContent}");
                    await LoadProductTypes();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to create product. " + ex.Message);
                await LoadProductTypes();
                return Page();
            }
        }

        private async Task LoadProductTypes()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/ProductTypes");
                
                if (response.IsSuccessStatusCode)
                {
                    var productTypes = await response.Content.ReadFromJsonAsync<List<ProductTypeDto>>();
                    ProductTypes = new SelectList(productTypes, "Id", "Name");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to load product types. Server returned an error.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Failed to load product types. " + ex.Message);
            }
        }
    }
}
