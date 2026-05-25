using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("cooperatives")]
public class Cooperative
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string Status { get; set; } = string.Empty; // pending, approved, rejected

    [Required]
    [Column("created_by")]
    public int CreatedBy { get; set; }

    [Column("staff_note")]
    public string? StaffNote { get; set; }

    [Column("reviewed_by")]
    public int? ReviewedBy { get; set; }

    [Column("reviewed_at")]
    public DateTime? ReviewedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CreatedBy")]
    public User CreatedByUser { get; set; } = null!;

    [ForeignKey("ReviewedBy")]
    public User? ReviewedByUser { get; set; }

    public ICollection<CooperativeMember> Members { get; set; } = new List<CooperativeMember>();
}
