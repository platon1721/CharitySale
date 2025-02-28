using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BLL.DTO;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Transactions
{
    public class IndexModel : AuthenticatedPageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public IList<MoneyTransactionDto> MoneyTransactions { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/MoneyTransactions");
            
            if (response.IsSuccessStatusCode)
            {
                MoneyTransactions = await response.Content.ReadFromJsonAsync<List<MoneyTransactionDto>>();
            }
            else
            {
                await response.Content.ReadAsStringAsync();
                MoneyTransactions = new List<MoneyTransactionDto>();
            }
        }
    }
}