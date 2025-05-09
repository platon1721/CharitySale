using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class ProductType: BaseDeletableEntity
{
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

}