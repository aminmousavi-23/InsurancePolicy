using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;

namespace PolicyService.Services.Interfaces
{
    public interface IPolicyTypeRepository
    {
        Task<BaseResponse<IList<PolicyTypeVm>>> GetAllAsync();
        Task<BaseResponse<PolicyTypeVm>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(PolicyTypeDto policyTypeDto);
        Task<BaseResponse> UpdateAsync(PolicyTypeDto policyTypeDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
