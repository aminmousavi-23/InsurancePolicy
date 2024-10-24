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
    public class RoleRepository(UserContext context, IMapper mapper) : IRoleRepository
    {
        private readonly UserContext _context = context;
        private readonly IMapper _mapper = mapper;

        #region GetAllAsync
        public async Task<BaseResponse<IList<RoleVm>>> GetAllAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return new BaseResponse<IList<RoleVm>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<IList<RoleVm>>(roles)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IList<RoleVm>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<BaseResponse<RoleVm>> GetByIdAsync(Guid id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return new BaseResponse<RoleVm>
                    {
                        IsSuccess = false,
                        Message = "Role was not found",
                        Result = null
                    };
                return new BaseResponse<RoleVm>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<RoleVm>(role)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<RoleVm>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region AddAsync
        public async Task<BaseResponse> AddAsync(RoleDto roleDto)
        {
            try
            {
                var validator = new RoleValidator();
                var validatorResult = await validator.ValidateAsync(roleDto);

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


                var existedRole = await _context.Roles
                    .AnyAsync(w => w.RoleName == roleDto.RoleName);
                if (existedRole)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This role already existed",
                        Result = null
                    };

                var newRole = _mapper.Map<Role>(roleDto);

                newRole.Id = Guid.NewGuid();

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
        #endregion

        #region UpdateAsync
        public async Task<BaseResponse> UpdateAsync(RoleDto roleDto)
        {
            try
            {
                var validator = new RoleValidator();
                var validatorResult = await validator.ValidateAsync(roleDto);

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
        #endregion

    }
}
