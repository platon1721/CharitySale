using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class UpdateReceiptProductDto
{
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}