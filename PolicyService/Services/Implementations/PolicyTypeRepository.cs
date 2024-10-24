using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PolicyService.Data;
using PolicyService.Entities;
using PolicyService.Models.DTOs;
using PolicyService.Models.DTOs.Validators;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;
using PolicyService.Services.Interfaces;

namespace PolicyService.Services.Implementations
{
    public class PolicyTypeRepository(PolicyContext context, IMapper mapper) : IPolicyTypeRepository
    {
        private readonly PolicyContext _context = context;
        private readonly IMapper _mapper = mapper;

        #region GetAllAsync
        public async Task<BaseResponse<IList<PolicyTypeVm>>> GetAllAsync()
        {
            try
            {
                var policyTypes = await _context.PolicyTypes.ToListAsync();
                if (policyTypes == null)
                    return new BaseResponse<IList<PolicyTypeVm>>
                    {
                        IsSuccess = false,
                        Message = "There is no Policy Types",
                        Result = null
                    };
                return new BaseResponse<IList<PolicyTypeVm>>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<IList<PolicyTypeVm>>(policyTypes)
                };
            }
            catch(Exception ex)
            {
                return new BaseResponse<IList<PolicyTypeVm>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region GetByIdAsync
        public async Task<BaseResponse<PolicyTypeVm>> GetByIdAsync(Guid id)
        {
            try
            {
                var policyType = await _context.PolicyTypes.FindAsync(id);
                if (policyType == null)
                    return new BaseResponse<PolicyTypeVm>
                    {
                        IsSuccess = false,
                        Message = "There is no Policy Type with this id",
                        Result = null
                    };

                return new BaseResponse<PolicyTypeVm>
                {
                    IsSuccess = true,
                    Message = "",
                    Result = _mapper.Map<PolicyTypeVm>(policyType)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PolicyTypeVm>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Result = null
                };
            }
        }
        #endregion

        #region AddAsync
        public async Task<BaseResponse> AddAsync(PolicyTypeDto policyTypeDto)
        {
            try
            {
                var validator = new PolicyTypeValidator();
                var validatorResult = await validator.ValidateAsync(policyTypeDto);

                if (validatorResult.Errors.Any())
                {
                    var errors = new List<string>();
                    foreach(var error in validatorResult.Errors)
                        errors.Add(error.ErrorMessage);

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "BadRequest",
                        Result = errors
                    };
                }

                var existedPolicyType = await _context.PolicyTypes
                .AnyAsync(w => w.Name == policyTypeDto.Name);
                if (existedPolicyType)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "This Policy Type already existed",
                        Result = null
                    };

                var newPolicyType = _mapper.Map<PolicyType>(policyTypeDto);

                newPolicyType.Id = Guid.NewGuid();

                await _context.AddAsync(newPolicyType);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Policy Type has been created successfully",
                    Result = $"Policy Type Id : {newPolicyType.Id}"
                };
            }
            catch(Exception ex)
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
        public async Task<BaseResponse> UpdateAsync(PolicyTypeDto policyTypeDto)
        {
            try
            {
                var validator = new PolicyTypeValidator();
                var validatorResult = await validator.ValidateAsync(policyTypeDto);

                if (validatorResult.Errors.Any())
                {
                    var errors = new List<string>();
                    foreach (var error in validatorResult.Errors)
                        errors.Add(error.ErrorMessage);

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "BadRequest",
                        Result = errors
                    };
                }

                var policyType = await _context.PolicyTypes
                .FindAsync(policyTypeDto.Id);
                if (policyType == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Policy Type was not found",
                        Result = null
                    };

                _mapper.Map(policyTypeDto, policyType);

                _context.PolicyTypes.Update(policyType);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Policy Type has been updated successfully",
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
                var policyType = await _context.PolicyTypes
                .FindAsync(id);
                if (policyType == null)
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Policy Type was not found",
                        Result = null
                    };

                _context.Remove(policyType);
                await _context.SaveChangesAsync();

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "PolicyType has been deleted successfully",
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
