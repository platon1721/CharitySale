using System.ComponentModel.DataAnnotations;

namespace BLL.DTO;

/// <summary>
/// Data Transfer Object (DTO) for creating a new user.
/// </summary>
public class CreateUserDto
{
    
    /// <summary>
    /// Gets or sets the user's first name. Maximum length is 128 characters.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Firstname { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the user's surname. Maximum length is 128 characters.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Surname { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the user's login. Maximum length is 128 characters.
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string Login { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets a value indicating whether the user is an administrator. Default is false.
    /// </summary>
    public bool IsAdmin { get; set; } = false;
}