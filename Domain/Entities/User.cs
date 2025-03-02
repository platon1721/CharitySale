using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User: BaseDeletableEntity
{
    
    [MaxLength (128)]
    public string Firstname { get; set; } = null!;
    
    [MaxLength(128)]
    public string Surname { get; set; } = null!;
    
    [MaxLength(50)]
    public string Login { get; set; } = default!;
    
    public bool IsAdmin { get; set; } = false;
    
    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
}