using BLL.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Users;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginModel(
        IHttpClientFactory httpClientFactory, 
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public string Login { get; set; }
    
    public IActionResult OnGetLogout()
    {
        HttpContext.Session.Clear();
        foreach (var cookie in Request.Cookies.Keys)
        {
            Response.Cookies.Delete(cookie);
        }
        HttpContext.SignOutAsync();
        
        return RedirectToPage("/Users/Login");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("WebApi");

        try 
        {
            var response = await httpClient.GetAsync($"api/Users/by-login/{Login}");
        
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<UserDto>();
            
                _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
                _httpContextAccessor.HttpContext.Session.SetString("UserLogin", user.Login);
                _httpContextAccessor.HttpContext.Session.SetString("UserFullName", user.FullName);
                _httpContextAccessor.HttpContext.Session.SetString("UserIsAdmin", user.IsAdmin.ToString());
            
                return RedirectToPage("/Index");
            }
        
            ModelState.AddModelError(string.Empty, "No such user");
            return Page();
        }
        catch 
        {
            ModelState.AddModelError(string.Empty, "Login error");
            return Page();
        }
    }
}