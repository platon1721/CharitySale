using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Pages.Shared;

namespace WebApp.Pages
{
    public class ImportProductsModel : AdminPageModel
    {
        private readonly HttpClient _httpClient;

        public ImportProductsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        [BindProperty]
        public IFormFile ConfigFile { get; set; }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public ImportResult ImportResult { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostUploadFileAsync()
    {
        if (ConfigFile == null || ConfigFile.Length == 0)
        {
            ErrorMessage = "Please select a valid file.";
            return Page();
        }

        if (!ConfigFile.FileName.EndsWith(".json"))
        {
            ErrorMessage = "Only JSON files are supported.";
            return Page();
        }

        try
        {
            // Convert IFormFile to content for HttpClient
            using var content = new MultipartFormDataContent();
            using var fileStream = ConfigFile.OpenReadStream();
            using var streamContent = new StreamContent(fileStream);
            content.Add(streamContent, "file", ConfigFile.FileName);

            var response = await _httpClient.PostAsync("api/ProductImport/upload", content);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Proovi kõigepealt otse deserializeerida
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ImportResult = System.Text.Json.JsonSerializer.Deserialize<ImportResult>(
                        jsonString, 
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
                catch (Exception deserializeEx)
                {
                    // Kui otse deserialiseerimine ebaõnnestub, kasuta manuaalset meetodit
                    Console.WriteLine($"Direct deserialization failed: {deserializeEx.Message}. Using manual method.");
                
                    var jsonData = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonDocument>();
                    ImportResult = new ImportResult
                    {
                        Success = jsonData.RootElement.GetProperty("success").GetBoolean(),
                        TotalItems = jsonData.RootElement.GetProperty("totalItems").GetInt32(),
                        AddedItems = jsonData.RootElement.GetProperty("addedItems").GetInt32(),
                        UpdatedItems = jsonData.RootElement.GetProperty("updatedItems").GetInt32(),
                        SkippedItems = jsonData.RootElement.GetProperty("skippedItems").GetInt32()
                    };

                    // Kontrolli valikulisi välju
                    if (jsonData.RootElement.TryGetProperty("errorMessage", out var errorMsg))
                    {
                        ImportResult.ErrorMessage = errorMsg.GetString();
                    }

                    if (jsonData.RootElement.TryGetProperty("skippedItemDetails", out var skippedItems))
                    {
                        ImportResult.SkippedItemDetails = new List<string>();
                        foreach (var item in skippedItems.EnumerateArray())
                        {
                            ImportResult.SkippedItemDetails.Add(item.GetString());
                        }
                    }
                }
            
                SuccessMessage = "Products imported successfully!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to import products: {error}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }

        public async Task<IActionResult> OnPostImportDefaultAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("api/ProductImport/from-default", null);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Proovi kõigepealt otse deserializeerida
                    var jsonString = await response.Content.ReadAsStringAsync();
                    ImportResult = System.Text.Json.JsonSerializer.Deserialize<ImportResult>(
                        jsonString, 
                        new System.Text.Json.JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                }
                catch (Exception deserializeEx)
                {
                    // Kui otse deserialiseerimine ebaõnnestub, kasuta manuaalset meetodit
                    Console.WriteLine($"Direct deserialization failed: {deserializeEx.Message}. Using manual method.");
                
                    var jsonData = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonDocument>();
                    ImportResult = new ImportResult
                    {
                        Success = jsonData.RootElement.GetProperty("success").GetBoolean(),
                        TotalItems = jsonData.RootElement.GetProperty("totalItems").GetInt32(),
                        AddedItems = jsonData.RootElement.GetProperty("addedItems").GetInt32(),
                        UpdatedItems = jsonData.RootElement.GetProperty("updatedItems").GetInt32(),
                        SkippedItems = jsonData.RootElement.GetProperty("skippedItems").GetInt32()
                    };

                    // Kontrolli valikulisi välju
                    if (jsonData.RootElement.TryGetProperty("errorMessage", out var errorMsg))
                    {
                        ImportResult.ErrorMessage = errorMsg.GetString();
                    }

                    if (jsonData.RootElement.TryGetProperty("skippedItemDetails", out var skippedItems))
                    {
                        ImportResult.SkippedItemDetails = new List<string>();
                        foreach (var item in skippedItems.EnumerateArray())
                        {
                            ImportResult.SkippedItemDetails.Add(item.GetString());
                        }
                    }
                }
            
                SuccessMessage = "Products imported successfully from default configuration!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                ErrorMessage = $"Failed to import products: {error}";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
}
    }
}