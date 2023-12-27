using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("Items")]
public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int LocationId { get; set; }

    [Required]
    [DefaultValue(0)]
    public int CurrentQuantity { get; set; } = 0;

    [Required]
    [DefaultValue(0)]
    public int OptimalQuantity { get; set; } = 0;

    [Required]
    [DefaultValue(0)]
    public decimal PricePerItem { get; set; } = 0;

    // Navigation properties
    public virtual ItemCategory Category { get; set; }
    public virtual ItemLocation Location { get; set; }
}