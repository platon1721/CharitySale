using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ReceiptProduct
{
    [Key]
    public int ReceiptId { get; set; }
    [Key]
    public int ProductId { get; set; }
    
    [Range(0, int.MaxValue)] public int Quantity { get; set; }
    [Range(0, int.MaxValue)] public decimal UnitPrice { get; set; }
    
    public virtual Receipt Receipt { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}