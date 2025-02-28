namespace BLL.DTO;

public class ReceiptDto : BaseDto
{
    public string UserFullName { get; set; } = default!;
    public decimal PaidAmount { get; set; }
    public List<ReceiptProductDto> Products { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsOpen { get; set; } = true;
    public bool IsReturned { get; set; } = false;
}
