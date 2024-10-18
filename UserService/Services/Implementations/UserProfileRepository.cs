using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class UserProfileRepository(UserContext context, IMapper mapper) : IUserProfileRepository
    {
        private readonly UserContext _context = context;
        private readonly IMapper _mapper = mapper;

        #region GetAllAsync
        public async Task<BaseResponse<IList<UserProfileVm>>> GetAllAsync()
        {
            try
            {
                var userProfiles = await _context.UserProfiles.ToListAsync();
                if (userProfiles == null)
                    return new BaseResponse<IList<UserProfileVm>>
                    {
                        IsSuccess = false,
                        Message = "There is no UserProfiles",
                        Result = null
                    };
                return new BaseResponse<IList<UserProfileVm>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<IList<UserProfileVm>>(userProfiles)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<UserProfileVm>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<BaseResponse<UserProfileVm>> GetByIdAsync(Guid id)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(id);
                if (userProfile == null)
                    return new BaseResponse<UserProfileVm>
                    {
                        IsSuccess = false,
                        Message = "UserProfile was not found",
                        Result = null
                    };
                return new BaseResponse<UserProfileVm>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<UserProfileVm>(userProfile)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserProfileVm>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region AddAsync
        public async Task<BaseResponse> AddAsync(UserProfileDto userProfileDto)
        {
            try
            {
                var existedUserProfile = await _context.UserProfiles
                    .AnyAsync(w => w.UserId == userProfileDto.UserId);
                if (existedUserProfile == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This UserProfile already existed",
                        Result = null
                    };

                var newUserProfile = _mapper.Map<UserProfile>(userProfileDto);

                newUserProfile.Id = Guid.NewGuid();

                await _context.UserProfiles.AddAsync(newUserProfile);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "UserProfile has been created successfuly",
                    Result = $"UserProfile Id : {newUserProfile.Id}"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region UpdateAsync
        public async Task<BaseResponse> UpdateAsync(UserProfileDto userProfileDto)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(userProfileDto.Id);
                if (userProfile == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "UserProfile was not found",
                        Result = null
                    };

                _mapper.Map(userProfileDto, userProfile);

                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "UserProfile has been updated successfully",
                    Result = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region DeleteAsync
        public async Task<BaseResponse> DeleteAsync(Guid id)
        {
            try
            {
                var userProfile = await _context.UserProfiles.FindAsync(id);
                if (userProfile == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "UserProfile was not found",
                        Result = null
                    };

                _context.Remove(userProfile);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "UserProfile has been deleted successfully",
                    Result = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion
    }
}
