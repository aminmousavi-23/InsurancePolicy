using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.DTOs.Validators;
using UserService.Models.ViewModels;
using UserService.Responses;
using UserService.Services.Interfaces;

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
                    .Include(u => u.Role)
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
                    .Include(u => u.Role)
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
        public async Task<BaseResponse> AddAsync(UserDto userDto)
        {
            try
            {
                var validator = new UserValidator();
                var validatorResult = await validator.ValidateAsync(userDto);

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


                var existedPhoneNumber = await _context.Users
                    .AnyAsync(w => w.PhoneNumber == userDto.PhoneNumber);
                if (existedPhoneNumber == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User with this phone number already existed",
                        Result = null
                    };

                var existedEmail = await _context.Users
                    .AnyAsync(w => w.Email == userDto.Email);
                if (existedEmail == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User with this email already existed",
                        Result = null
                    };

                var existedRole = await _context.Roles
                    .AnyAsync(w => w.Id == userDto.RoleId);
                if (existedRole == false)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This role is not existed",
                        Result = null
                    };

                var newUser = _mapper.Map<User>(userDto);

                newUser.Id = Guid.NewGuid();

                newUser.HashedPassword = _passwordHasherService.HashPassword(userDto.Password);

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
        public async Task<BaseResponse> UpdateAsync(UserDto userDto)
        {
            try
            {
                var validator = new UserValidator();
                var validatorResult = await validator.ValidateAsync(userDto);

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


                var user = await _context.Users.FindAsync(userDto.Id);
                if (user == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        Result = null
                    };

                var existedRole = await _context.Roles
                    .AnyAsync(w => w.Id == userDto.RoleId);
                if (existedRole == false)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This role is not existed",
                        Result = null
                    };

                _mapper.Map(userDto, user);

                user.HashedPassword = _passwordHasherService.HashPassword(userDto.Password);

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
