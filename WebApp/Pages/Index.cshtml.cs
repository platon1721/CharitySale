using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using WebApp.Pages.Shared;
using BLL.DTO;

namespace WebApp.Pages;

public class IndexModel : AuthenticatedPageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly HttpClient _httpClient;
    
    public string UserFullName { get; private set; } = "User";

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("WebApi");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (UserId.HasValue)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Users/{UserId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDto>();
                    if (user != null)
                    {
                        UserFullName = user.FullName;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load user information");
            }
        }

        return Page();
    }
}