using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;

namespace UserService.Services.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<BaseResponse<IList<UserProfileVm>>> GetAllAsync();
        Task<BaseResponse<UserProfileVm>> GetByIdAsync(Guid id);
        Task<BaseResponse> AddAsync(UserProfileDto userProfileDto);
        Task<BaseResponse> UpdateAsync(UserProfileDto userProfileDto);
        Task<BaseResponse> DeleteAsync(Guid id);
    }
}
