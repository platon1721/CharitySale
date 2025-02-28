using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Receipt: BaseEntity
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [Range(0, int.MaxValue)] public decimal PaidAmount { get; set; } = 0;
    
    public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; } = new List<ReceiptProduct>();

    
}