using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;

namespace UserService.Services.Interfaces;

public interface IUserRoleRepository
{
    Task<BaseResponse<IList<UserRoleVm>>> GetAllAsync();
    Task<BaseResponse<UserRoleVm>> GetByIdAsync(Guid id);
    Task<BaseResponse> AddAsync(UserRoleDto userRoleDto);
    Task<BaseResponse> UpdateAsync(UserRoleDto userRoleDto);
    Task<BaseResponse> DeleteAsync(Guid id);
}
