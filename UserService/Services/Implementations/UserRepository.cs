using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Entities;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class UserRepository(UserContext context, IMapper mapper, IPasswordHasherService passwordHasherService) : IUserRepository
    {
        private readonly UserContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasherService _passwordHasherService = passwordHasherService;

        public async Task<BaseResponse<IList<User>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                if (users == null)
                    return new BaseResponse<IList<User>>
                    {
                        IsSuccess = false,
                        Message = "There is no Users",
                        Result = null
                    };
                return new BaseResponse<IList<User>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = users
                };
            }
            catch(Exception ex) 
            {
                return new BaseResponse<IList<User>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }

        public async Task<BaseResponse<User>> GetByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return new BaseResponse<User>
                    {
                        IsSuccess = false,
                        Message = "User was not found",
                        Result = null
                    };
                return new BaseResponse<User>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        public async Task<BaseResponse> AddAsync(UserDto userDto)
        {
            try
            {
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

                var newUser = _mapper.Map<User>(userDto);               

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
        public async Task<BaseResponse> UpdateAsync(UserDto userDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(userDto.Id);
                if (user == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "User was not found",
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
                    Result = $"User Id : {user.Id}"
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
    }
}
