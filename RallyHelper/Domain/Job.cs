using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

[Table("Jobs")]
public class Job
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public decimal? TotalPrice { get; set; }
    public int? RallyId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    [Required]
    public string ClientName { get; set; }

    // Navigation properties
    public virtual ICollection<JobItem> JobItems { get; set; }
}