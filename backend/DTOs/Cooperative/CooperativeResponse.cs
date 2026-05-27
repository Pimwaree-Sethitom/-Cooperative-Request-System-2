namespace backend.DTOs.Cooperative;

public class CooperativeMemberResponse
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? NationalId { get; set; }
    public string? Phone { get; set; }
}

public class CooperativeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public List<CooperativeMemberResponse> Members { get; set; } = new();
}
