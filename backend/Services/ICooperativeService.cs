using backend.DTOs.Cooperative;

namespace backend.Services;

public interface ICooperativeService
{
    Task<CooperativeResponse> CreateCooperativeAsync(int userId, CreateCooperativeRequest request);
    Task<List<CooperativeResponse>> GetMyCooperativesAsync(int userId);
    Task<List<CooperativeResponse>> GetAllCooperativesAsync(string? status);
    Task<CooperativeResponse> ReviewCooperativeAsync(int staffId, int cooperativeId, ReviewCooperativeRequest request);
}
