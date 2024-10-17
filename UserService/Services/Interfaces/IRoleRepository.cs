using UserService.DTOs;
using UserService.Entities;
using UserService.Responses;

namespace UserService.Services.Interfaces
{
    public interface IRoleRepository
    {
        Task<BaseResponse<IList<Role>>> GetAllAsync();
        Task<BaseResponse<Role>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(RoleDto roleDto);
        Task<BaseResponse> UpdateAsync(RoleDto roleDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
