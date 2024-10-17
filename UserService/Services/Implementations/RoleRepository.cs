using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.DTOs;
using UserService.Entities;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class RoleRepository(UserContext context, IMapper mapper) : IRoleRepository
    {
        private readonly UserContext _context = context;
        private readonly IMapper _mapper = mapper;
        public async Task<BaseResponse<IList<Role>>> GetAllAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                if (roles == null)
                    return new BaseResponse<IList<Role>>
                    {
                        IsSuccess = false,
                        Message = "There is no Roles",
                        Result = null
                    };
                return new BaseResponse<IList<Role>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = roles
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<Role>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }

        public async Task<BaseResponse<Role>> GetByIdAsync(Guid id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return new BaseResponse<Role>
                    {
                        IsSuccess = false,
                        Message = "Role was not found",
                        Result = null
                    };
                return new BaseResponse<Role>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = role
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Role>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        public async Task<BaseResponse> AddAsync(RoleDto roleDto)
        {
            try
            {
                var existedRole = await _context.Roles
                    .AnyAsync(w => w.RoleName == roleDto.RoleName);
                if (existedRole == true)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This role already existed",
                        Result = null
                    };

                var newRole = _mapper.Map<Role>(roleDto);

                await _context.Roles.AddAsync(newRole);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Role has been created successfuly",
                    Result = $"Role Id : {newRole.Id}"
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
        public async Task<BaseResponse> UpdateAsync(RoleDto roleDto)
        {
            try
            {
                var role = await _context.Roles.FindAsync(roleDto.Id);
                if (role == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Role was not found",
                        Result = null
                    };

                _mapper.Map(roleDto, role);

                _context.Roles.Update(role);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Role has been updated successfully",
                    Result = $"Role Id : {role.Id}"
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
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Role was not found",
                        Result = null
                    };

                _context.Remove(role);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Role has been deleted successfully",
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
