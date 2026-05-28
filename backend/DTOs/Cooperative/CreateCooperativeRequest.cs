using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Cooperative;

public class CreateCooperativeRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; } //optional

    [Required(ErrorMessage = "Members are required")]
    public List<CooperativeMemberRequest> Members { get; set; } = new();
}
