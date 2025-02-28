namespace BLL.DTO;

public class UserDto : BaseDto
{
    public string Firstname { get; set; } = default!;
    public string Surname { get; set; } = default!;
    
    public string Login { get; set; } = default!;

    public bool IsAdmin { get; set; } = false;
    public string FullName => $"{Firstname} {Surname}";
}
