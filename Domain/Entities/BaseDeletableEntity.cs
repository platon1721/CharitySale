namespace Domain.Entities;

/// <summary>
/// Abstract class, that adds soft delete functionality.
/// </summary>
public abstract class BaseDeletableEntity : BaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
}