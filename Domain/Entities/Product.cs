using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product: BaseDeletableEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = null!;
    
    [Range(0, int.MaxValue)] public decimal Price { get; set; } = 0;
    
    public int ProductTypeId { get; set; }
    public virtual ProductType ProductType { get; set; } = null!;
    
    [MaxLength(255)] public string Description { get; set; } = null!;

    [Range(0, int.MaxValue)] public int Stock { get; set; } = 0;
    
    [MaxLength(2048)] public string? ImageUrl { get; set; }

    [Timestamp] public byte[] RowVersion { get; set; } = null!;
    
    public bool IsInStock => Stock > 0;
    
    public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; } = new List<ReceiptProduct>();
}