using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class UpdateReceiptProductQuantityDto
{
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}