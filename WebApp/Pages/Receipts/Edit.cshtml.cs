using BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApp.Pages.Shared;

namespace WebApp.Pages.Receipts
{
    public class EditModel : AuthenticatedPageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [BindProperty] public ReceiptDto Receipt { get; set; }

        public List<ProductTypeDto> ProductTypes { get; set; }
        public List<ProductDto> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Receipts/{id}");
            if (response.IsSuccessStatusCode)
            {
                Receipt = await response.Content.ReadFromJsonAsync<ReceiptDto>();
            }
            else
            {
                return NotFound();
            }

            await LoadProductsAndTypes();
            return Page();
        }

        private async Task LoadProductsAndTypes()
        {
            var productTypesResponse = await _httpClient.GetAsync("api/ProductTypes");
            ProductTypes = await productTypesResponse.Content.ReadFromJsonAsync<List<ProductTypeDto>>();
            
            var productsResponse = await _httpClient.GetAsync("api/Products");
            Products = await productsResponse.Content.ReadFromJsonAsync<List<ProductDto>>();
        }

        public async Task<IActionResult> OnPostAddProductAsync(int receiptId, int productId, int quantity)
        {
            try 
            {
                var dto = new AddProductToReceiptDto
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                var response = await _httpClient.PostAsJsonAsync($"api/Receipts/{receiptId}/products", dto);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return RedirectToPage("./Edit", new { id = receiptId });
                }
        
                var errorContent = await response.Content.ReadAsStringAsync();
        
                ModelState.AddModelError(string.Empty, "Failed to add product to receipt");
                await LoadProductsAndTypes();
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while adding the product");
                await LoadProductsAndTypes();
                return Page();
            }
        }
        
        /// <summary>
        /// Method to delete product from the open receipt.
        /// </summary>
        /// <param name="receiptId">Unique identifier of the receipt, where from to delete the product.</param>
        /// <param name="productId">Unique identifier of the product to be deleted.</param>
        /// <returns>If task was successful, redirect to the open receipt edit page.</returns>
        public async Task<IActionResult> OnPostRemoveProductAsync(int receiptId, int productId)
        {
            try 
            {
                var response = await _httpClient.DeleteAsync($"api/Receipts/{receiptId}/products/{productId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Edit", new { id = receiptId });
                }

                await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, "Failed to remove product from receipt");
                await LoadProductsAndTypes();
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while removing the product");
                await LoadProductsAndTypes();
                return Page();
            }
        }

        /// <summary>
        /// Method that changes product quantity in open bill,
        /// </summary>
        /// <param name="receiptId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostUpdateProductQuantityAsync(int receiptId, int productId, int quantity)
        {
            try 
            {
                var dto = new UpdateReceiptProductDto
                {
                    ProductId = productId,
                    Quantity = quantity
                };

                var requestContent = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(dto), 
                    System.Text.Encoding.UTF8, 
                    "application/json"
                );

                var response = await _httpClient.PutAsync(
                    $"api/Receipts/{receiptId}/products/{productId}", 
                    requestContent
                );
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Edit", new { id = receiptId });
                }
                
                ModelState.AddModelError(string.Empty, "Failed to update product quantity");
                await LoadProductsAndTypes();
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating product quantity");
                await LoadProductsAndTypes();
                return Page();
            }
        }

        /// <summary>
        /// Method to cancel open bill.
        /// Method deletes bill by id.
        /// </summary>
        /// <param name="receiptId">Unique identifier of the receipt to be deleted</param>
        /// <returns>If task was not successful, returns same page, otherwise, redirects to the POS page.</returns>
        public async Task<ActionResult> OnPostCancelReceiptAsync(int receiptId)
        {
            try
            {
                await _httpClient.DeleteAsync($"api/Receipts/{receiptId}");
                return RedirectToPage("./Index");
            }
            catch (Exception)
            {
                return Page();
            }
        }

        /// <summary>
        /// Completes the receipt. Create new sale transaction.
        /// </summary>
        /// <param name="receiptId"></param>
        /// <returns></returns>
        public async Task<ActionResult> OnPostCompleteReceiptAsync(int receiptId, decimal amount)
        {
            try
            {
            
                var dto = new 
                { 
                    ReceiptId = receiptId, 
                    Amount = amount 
                };
                
                var response = await _httpClient.PostAsJsonAsync($"api/MoneyTransactions/sale", dto);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Receipts/Index");
                }
                else
                {
                    await response.Content.ReadAsStringAsync();

                    ModelState.AddModelError(string.Empty, "Failed to complete receipt");
                    return Page();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while completing the receipt");
                return Page();
            }
        }
    }
}
