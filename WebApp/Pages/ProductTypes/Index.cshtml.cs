using BLL.DTO;
using WebApp.Pages.Shared;

namespace WebApp.Pages.ProductTypes
{
    public class IndexModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public IList<ProductTypeDto> ProductTypes { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductTypes = await _httpClient.GetFromJsonAsync<List<ProductTypeDto>>("api/ProductTypes") 
                           ?? new List<ProductTypeDto>();
        }
    }
}
