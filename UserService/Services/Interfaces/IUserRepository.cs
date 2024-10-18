using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;

namespace UserService.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<BaseResponse<IList<UserVm>>> GetAllAsync();
        Task<BaseResponse<UserVm>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(UserDto userDto);
        Task<BaseResponse> UpdateAsync(UserDto userDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
