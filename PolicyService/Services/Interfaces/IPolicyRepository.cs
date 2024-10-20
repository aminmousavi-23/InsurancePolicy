using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;

namespace PolicyService.Services.Interfaces
{
    public interface IPolicyRepository
    {
        Task<BaseResponse<IList<PolicyVm>>> GetAllAsync();
        Task<BaseResponse<PolicyVm>> GetByPolicyNumberAsync(string policyNumber);
        Task<BaseResponse<PolicyVm>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(PolicyDto policyDto);
        Task<BaseResponse> UpdateAsync(PolicyDto policyDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
