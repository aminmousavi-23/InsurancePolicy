using UserService.DTOs;
using UserService.Entities;
using UserService.Responses;

namespace UserService.Services.Interfaces
{
    public interface IUserRepository
    {
        Task<BaseResponse<IList<User>>> GetAllAsync();
        Task<BaseResponse<User>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(UserDto userDto);
        Task<BaseResponse> UpdateAsync(UserDto userDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
