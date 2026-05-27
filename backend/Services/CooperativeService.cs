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

    private static CooperativeResponse MapToResponse(Cooperative c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Description = c.Description,
        Status = c.Status,
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
