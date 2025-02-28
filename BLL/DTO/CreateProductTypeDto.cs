using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class CreateProductTypeDto
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = default!;
}