namespace BLL.DTO;


/// <summary>
/// Data Transfer Object (DTO) representing a user.
/// </summary>
public class UserDto : BaseDeletableDto
{
    
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string Firstname { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the surname of the user.
    /// </summary>
    public string Surname { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the login name of the user.
    /// </summary>
    public string Login { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the user has administrator privileges. False by default.
    /// </summary>
    public bool IsAdmin { get; set; } = false;
    
    /// <summary>
    /// Gets the full name of the user (Firstname + Surname).
    /// </summary>
    public string FullName => $"{Firstname} {Surname}";
}
