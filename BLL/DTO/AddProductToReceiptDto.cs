using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class AddProductToReceiptDto
{
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}