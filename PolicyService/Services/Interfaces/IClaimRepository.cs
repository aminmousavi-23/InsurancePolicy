using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;

namespace PolicyService.Services.Interfaces;

public interface IClaimRepository
{
    Task<BaseResponse<IList<ClaimVm>>> GetAllAsync();
    Task<BaseResponse<ClaimVm>> GetByClaimNumberAsync(string claimNumber);
    Task<BaseResponse<ClaimVm>> GetByIdAsync(Guid id);
    Task<BaseResponse> AddAsync(ClaimDto claimDto);
    Task<BaseResponse> UpdateAsync(ClaimDto claimDto);
    Task<BaseResponse> DeleteAsync(Guid id);
}
