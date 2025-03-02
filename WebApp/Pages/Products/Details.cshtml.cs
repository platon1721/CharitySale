using System.Text.Json;
using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Products
{
    public class DetailsModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }
        
        [BindProperty]
        public ProductDto Product { get; set; } = default!;
        

        [BindProperty]
        public CreateProductDto EditProduct { get; set; } = new CreateProductDto();

        public List<ProductTypeDto> ProductTypes { get; set; } = new List<ProductTypeDto>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var response = await _httpClient.GetAsync($"api/Products/{id}");
                var productTypesResponse = await _httpClient.GetAsync("api/ProductTypes");

                if (response.IsSuccessStatusCode && productTypesResponse.IsSuccessStatusCode)
                {
                    Product = await response.Content.ReadFromJsonAsync<ProductDto>();
                    ProductTypes = await productTypesResponse.Content.ReadFromJsonAsync<List<ProductTypeDto>>();
                    
                    EditProduct = new CreateProductDto
                    {
                        Name = Product.Name,
                        Description = Product.Description,
                        Price = Product.Price,
                        ProductTypeId = Product.ProductTypeId
                    };
                    

                    return Page();
                }
        
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostUpdateProductAsync()
        {
            try
            { 
                var response = await _httpClient.PutAsJsonAsync($"api/Products/{Product.Id}", EditProduct);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Details", new { id = Product.Id });
                }
                
                ModelState.AddModelError(string.Empty, "Failed to update the product");
                return Page();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the product");
                return Page();
            }
        }
        
        
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Products/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Details", new { id });
                }
                
                await response.Content.ReadAsStringAsync();
                

                ModelState.AddModelError(string.Empty, "Failed to delete the product");
                
                return Page();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the product");
                return Page();
            }
        }
        
    }
}
