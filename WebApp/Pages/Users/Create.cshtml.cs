using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Users
{
    public class CreateModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public CreateUserDto User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var response = await _httpClient.PostAsJsonAsync("api/Users", User);
            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, errorContent);
            return Page();
        }
    }
}
