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
        Task<BaseResponse> AddAsync(RegisterUserDto registerUserDto);
        Task<BaseResponse> UpdateAsync(UpdateUserDto updateUserDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
