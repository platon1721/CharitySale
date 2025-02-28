using BLL.DTO;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Users
{
    public class IndexModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public IList<UserDto> Users { get;set; } = default!;

        public async Task OnGetAsync()
        {
            try 
            {
                var response = await _httpClient.GetAsync("api/Users");
            
                if (response.IsSuccessStatusCode)
                {
                    Users = await response.Content.ReadFromJsonAsync<List<UserDto>>() 
                            ?? new List<UserDto>();
                }
                else 
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    Users = new List<UserDto>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Users = new List<UserDto>();
            }
        }
    }
}
