using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.ViewModels;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations;

public class UserRoleRepository(UserContext context, IMapper mapper) : IUserRoleRepository
{
    private readonly UserContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region GetAllAsync
    public async Task<BaseResponse<IList<UserRoleVm>>> GetAllAsync()
    {
        try
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            return new BaseResponse<IList<UserRoleVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<UserRoleVm>>(userRoles)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<UserRoleVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<UserRoleVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null)
                return new BaseResponse<UserRoleVm>
                {
                    IsSuccess = false,
                    Message = "User Role was not found",
                    Result = null
                };
            return new BaseResponse<UserRoleVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<UserRoleVm>(userRole)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<UserRoleVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(UserRoleDto userRole)
    {
        try
        {
            var existedUserRole = await _context.UserRoles
                .AnyAsync(w => w.UserId == userRole.UserId);
            if (existedUserRole)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This User already has a role",
                    Result = null
                };

            var newUserRole = _mapper.Map<UserRole>(userRole);
            

            await _context.UserRoles.AddAsync(newUserRole);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "User Role has been created successfuly",
                Result = $"User Role Id : {newUserRole.Id}"
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
    public async Task<BaseResponse> UpdateAsync(UserRoleDto userRoleDto)
    {
        try
        {
            var userRole = await _context.UserRoles.FindAsync(userRoleDto.Id);
            if (userRole == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User Role was not found",
                    Result = null
                };

            _mapper.Map(userRoleDto, userRole);

            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "User Role has been updated successfully",
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
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User Role was not found",
                    Result = null
                };

            _context.Remove(userRole);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "User Role has been deleted successfully",
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
