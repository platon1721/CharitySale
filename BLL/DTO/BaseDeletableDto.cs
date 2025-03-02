namespace BLL.DTO;

public class BaseDeletableDto : BaseDto
{
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}