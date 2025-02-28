using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class CreateReceiptDto
{
    [Required]
    public int UserId { get; set; }
    
    public List<CreateReceiptProductDto> Products { get; set; } = new();
}
