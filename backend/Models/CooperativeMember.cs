using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("cooperative_members")]
public class CooperativeMember
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("cooperative_id")]
    public int CooperativeId { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("national_id")]
    public string? NationalId { get; set; }

    [MaxLength(20)]
    [Column("phone")]
    public string? Phone { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CooperativeId")]
    public Cooperative Cooperative { get; set; } = null!;
}
