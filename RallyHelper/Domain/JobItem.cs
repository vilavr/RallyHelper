using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("JobItems")]
public class JobItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int JobId { get; set; }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemId { get; set; }

    [Required]
    [DefaultValue(0)]
    public int NeededQuantity { get; set; } = 0;

    // Navigation properties
    public virtual Job Job { get; set; }
    public virtual Item Item { get; set; }
}