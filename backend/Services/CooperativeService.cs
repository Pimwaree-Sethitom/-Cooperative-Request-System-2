using backend.Data;
using backend.DTOs.Cooperative;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class CooperativeService : ICooperativeService
{
    private readonly AppDbContext _context;

    public CooperativeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CooperativeResponse> CreateCooperativeAsync(int userId, CreateCooperativeRequest request)
    {
        if (request.Members.Count < 10)
            throw new InvalidOperationException("Initial members must be at least 10");

        var nameExists = await _context.Cooperatives.AnyAsync(c => c.Name == request.Name);
        if (nameExists)
            throw new InvalidOperationException("Cooperative name already exists");

        var cooperative = new Cooperative
        {
            Name = request.Name,
            Description = request.Description,
            Status = "pending",
            CreatedBy = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Members = request.Members.Select(m => new CooperativeMember
            {
                FullName = m.FullName,
                NationalId = m.NationalId,
                Phone = m.Phone,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        _context.Cooperatives.Add(cooperative);
        await _context.SaveChangesAsync();

        return MapToResponse(cooperative);
    }

    public async Task<List<CooperativeResponse>> GetMyCooperativesAsync(int userId)
    {
        var cooperatives = await _context.Cooperatives
            .Include(c => c.Members)
            .Where(c => c.CreatedBy == userId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return cooperatives.Select(MapToResponse).ToList();
    }

    public async Task<List<CooperativeResponse>> GetAllCooperativesAsync(string? status)
    {
        var query = _context.Cooperatives.Include(c => c.Members).AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(c => c.Status == status);

        var cooperatives = await query.OrderByDescending(c => c.CreatedAt).ToListAsync();
        return cooperatives.Select(MapToResponse).ToList();
    }

    public async Task<CooperativeResponse> ReviewCooperativeAsync(int staffId, int cooperativeId, ReviewCooperativeRequest request)
    {
        if (request.Action != "approve" && request.Action != "reject")
            throw new ArgumentException("Action must be 'approve' or 'reject'");

        var cooperative = await _context.Cooperatives
            .Include(c => c.Members)
            .FirstOrDefaultAsync(c => c.Id == cooperativeId)
            ?? throw new KeyNotFoundException("Cooperative not found");

        if (cooperative.Status != "pending")
            throw new InvalidOperationException("This request has already been reviewed");

        cooperative.Status = request.Action == "approve" ? "approved" : "rejected";
        cooperative.ReviewedBy = staffId;
        cooperative.ReviewedAt = DateTime.UtcNow;
        cooperative.StaffNote = request.StaffNote;
        cooperative.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(cooperative);
    }

    private static CooperativeResponse MapToResponse(Cooperative c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        Status = c.Status,
        StaffNote = c.StaffNote,
        ReviewedAt = c.ReviewedAt,
        CreatedAt = c.CreatedAt,
        Members = c.Members.Select(m => new CooperativeMemberResponse
        {
            Id = m.Id,
            FullName = m.FullName,
            NationalId = m.NationalId,
            Phone = m.Phone
        }).ToList()
    };
}
