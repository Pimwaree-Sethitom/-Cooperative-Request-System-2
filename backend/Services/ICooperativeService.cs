using backend.DTOs.Cooperative;

namespace backend.Services;

public interface ICooperativeService
{
    Task<CooperativeResponse> CreateCooperativeAsync(int userId, CreateCooperativeRequest request);
    Task<List<CooperativeResponse>> GetMyCooperativesAsync(int userId);
}
