using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.DTOs.Validators;
using UserService.Models.ViewModels;
using UserService.Responses;
using UserService.Services.Interfaces;
using UserService.Utilities;

namespace UserService.Services.Implementations
{
    public class UserRepository(UserContext context, IMapper mapper, IPasswordHasherService passwordHasherService)
        : IUserRepository
    {
        private readonly UserContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

        #region GetAllAsync
        public async Task<BaseResponse<IList<UserVm>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users
                    .ToListAsync();
                return new BaseResponse<IList<UserVm>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<IList<UserVm>>(users)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<UserVm>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<BaseResponse<UserVm>> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return new BaseResponse<UserVm>
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        Result = null
                    };
                return new BaseResponse<UserVm>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<UserVm>(user)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserVm>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region AddAsync
        public async Task<BaseResponse> AddAsync(RegisterUserDto registerUserDto)
        {
            try
            {
                var validator = new RegisterUserValidator();
                var validatorResult = await validator.ValidateAsync(registerUserDto);

                if (validatorResult.Errors.Any())
                {
                    var errors = new List<string>();
                    foreach (var error in validatorResult.Errors)
                        errors.Add(error.ErrorMessage);

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Bad Request",
                        Result = errors
                    };
                }


                var existedUser = await _context.Users
                    .AnyAsync(w => w.NationalCode == registerUserDto.NationalCode);
                if (existedUser == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User with this National Code already existed",
                        Result = null
                    };

                var existedEmail = await _context.Users
                    .AnyAsync(w => w.Email == registerUserDto.Email);
                if (existedEmail == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This email already existed",
                        Result = null
                    };

                var newUser = _mapper.Map<User>(registerUserDto);

                newUser.HashedPassword = _passwordHasherService.HashPassword(registerUserDto.Password);

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "User has been created successfuly",
                    Result = $"User Id : {newUser.Id}"
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
        public async Task<BaseResponse> UpdateAsync(UpdateUserDto updateUserDto)
        {
            try
            {
                var validator = new UpdateUserValidator();
                var validatorResult = await validator.ValidateAsync(updateUserDto);

                if (validatorResult.Errors.Any())
                {
                    var errors = new List<string>();
                    foreach (var error in validatorResult.Errors)
                        errors.Add(error.ErrorMessage);

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Bad Request",
                        Result = errors
                    };
                }


                var user = await _context.Users.FindAsync(updateUserDto.Id);
                if (user == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        Result = null
                    };

                var verifyPassword = _passwordHasherService.VerifyHashedPassword(updateUserDto, user.HashedPassword, updateUserDto.CurrentPassword);
                if (verifyPassword == "Failed")
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Current Password is not currect",
                        Result = null
                    };

                _mapper.Map(updateUserDto, user);

                user.UpdatedAt = DateTime.Now;
                user.HashedPassword = _passwordHasherService.HashPassword(updateUserDto.Password);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "User has been updated successfully",
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
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        Result = null
                    };

                _context.Remove(user);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "User has been deleted successfully",
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
