using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
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

            try 
            {
                var response = await _httpClient.GetAsync("api/Products/all");
            
                if (response.IsSuccessStatusCode)
                {
                    Products = await response.Content.ReadFromJsonAsync<List<ProductDto>>() 
                               ?? new List<ProductDto>();
                }
            }
            catch 
            {
                Products = new List<ProductDto>();
            }

            return Page();
        }
    }
}
