using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BLL.DTO;

public class CreateProductDto
{
    
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [Required]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
    
    [Required]
    public int ProductTypeId { get; set; }
    
    [Required]
    [MaxLength(255)]
    public string Description { get; set; } = default!;
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}