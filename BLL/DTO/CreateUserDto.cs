using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

public class CreateUserDto
{
    [Required]
    [MaxLength(128)]
    public string Firstname { get; set; } = default!;
    
    [Required]
    [MaxLength(128)]
    public string Surname { get; set; } = default!;
    
    [Required]
    [MaxLength(128)]
    public string Login { get; set; } = default!;
    
    public bool IsAdmin { get; set; } = false;
}