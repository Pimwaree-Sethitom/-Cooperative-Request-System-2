using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("full_name")]
    public string? FullName { get; set; }

    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("RoleId")]
    public Role Role { get; set; } = null!;

    public ICollection<Cooperative> CreatedCooperatives { get; set; } = new List<Cooperative>();
    public ICollection<Cooperative> ReviewedCooperatives { get; set; } = new List<Cooperative>();
}
