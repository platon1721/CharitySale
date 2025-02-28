using Microsoft.AspNetCore.Http;

namespace BLL.Services;

public interface IProductImportService
{
    Task<ImportResult> ImportProductsFromFileAsync(IFormFile configFile);
    Task<ImportResult> ImportProductsFromDefaultConfigAsync();
}

public class ImportResult
{
    public bool Success { get; set; }
    public int TotalItems { get; set; }
    public int AddedItems { get; set; }
    public int UpdatedItems { get; set; }
    public int SkippedItems { get; set; }
    public string ErrorMessage { get; set; }
    public List<string> SkippedItemDetails { get; set; } = new();
}