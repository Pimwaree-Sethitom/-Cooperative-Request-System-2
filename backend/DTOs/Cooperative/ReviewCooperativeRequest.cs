using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Cooperative;

public class ReviewCooperativeRequest
{
    [Required(ErrorMessage = "Action is required")]
    public string Action { get; set; } = string.Empty; // "approve" or "reject"

    public string? StaffNote { get; set; }
}
