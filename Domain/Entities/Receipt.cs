using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Receipt: BaseDeletableEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [Range(0, int.MaxValue)] public decimal PaidAmount { get; set; } = 0;
    
    public bool IsOpen { get; set; } = true;
    
    public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; } = new List<ReceiptProduct>();

    
}