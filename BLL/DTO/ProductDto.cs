namespace BLL.DTO;

public class ProductDto : BaseDto
{
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public string Description { get; set; } = default!;
    public int Quantity { get; set; }
    public string? ImageUrl { get; set; }
    public string ProductTypeName { get; set; } = default!;
    public bool IsInStock { get; set; }
    public int ProductTypeId { get; set; }
}