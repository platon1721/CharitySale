namespace BLL.DTO;

public class ProductStockDto : BaseDto
{
    public int Stock { get; set; }
    public bool IsInStock { get; set; }
}