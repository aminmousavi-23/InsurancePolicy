using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;

namespace UserService.Services.Interfaces
{
    public interface IRoleRepository
    {
        Task<BaseResponse<IList<RoleVm>>> GetAllAsync();
        Task<BaseResponse<RoleVm>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(RoleDto roleDto);
        Task<BaseResponse> UpdateAsync(RoleDto roleDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
