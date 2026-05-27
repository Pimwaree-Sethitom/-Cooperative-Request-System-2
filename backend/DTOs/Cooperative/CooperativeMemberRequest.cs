using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Cooperative;

public class CooperativeMemberRequest
{
    [Required(ErrorMessage = "FullName is required")]
    [MaxLength(255)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? NationalId { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }
}
