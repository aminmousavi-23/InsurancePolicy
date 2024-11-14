using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PolicyService.Data;
using PolicyService.Entities;
using PolicyService.Features;
using PolicyService.Models.DTOs;
using PolicyService.Models.DTOs.Validators;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;
using PolicyService.Services.Interfaces;
using System.Runtime.InteropServices;

namespace PolicyService.Services.Implementations;

public class PolicyRepository(PolicyContext context, IMapper mapper, IHttpClientFactory httpClientFactory) 
    : IPolicyRepository
{
    private readonly PolicyContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalService");

    #region GetAllAsync
    public async Task<BaseResponse<IList<PolicyVm>>> GetAllAsync()
    {
        try
        {
            var policies = await _context.Policies.ToListAsync();
            return new BaseResponse<IList<PolicyVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<PolicyVm>>(policies)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<PolicyVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion GetByPolicyNumberAsync

    #region GetByPolicyNumberAsync
    public async Task<BaseResponse<PolicyVm>> GetByPolicyNumberAsync(string policyNumber)
    {
        try
        {
            var policy = await _context.Policies
                .FirstOrDefaultAsync(w => w.PolicyNumber == policyNumber);
            if (policy == null)
                return new BaseResponse<PolicyVm>
                {
                    IsSuccess = false,
                    Message = "Policy was not found",
                    Result = null
                };
            return new BaseResponse<PolicyVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<PolicyVm>(policy)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<PolicyVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion 

    #region GetByIdAsync
    public async Task<BaseResponse<PolicyVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
                return new BaseResponse<PolicyVm>
                {
                    IsSuccess = false,
                    Message = "Policy was not found",
                    Result = null
                };
            return new BaseResponse<PolicyVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<PolicyVm>(policy)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<PolicyVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(PolicyDto policyDto)
    {
        try
        {
            var validator = new PolicyValidator();
            var validatorResult = await validator.ValidateAsync(policyDto);

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

            var userResponse = await _httpClient.GetAsync($"http://userservice:80/user/{policyDto.UserId}");
            var user = await userResponse.Content.ReadAsStringAsync();

            if (user == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User with this Id is not existed",
                    Result = null
                };


            var existedPolicyType = await _context.PolicyTypes
                .AnyAsync(w => w.Id == policyDto.PolicyTypeId);
            if (existedPolicyType == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This Policy Type is not existed",
                    Result = null
                };

            var newPolicy = _mapper.Map<Policy>(policyDto);

            newPolicy.Id = Guid.NewGuid();

            var generateUniqueNumber = new GenerateUniqueNumber();
            newPolicy.PolicyNumber = generateUniqueNumber.GeneratePolicyNumber();
            newPolicy.CreatedAt = DateTime.Now;

            await _context.Policies.AddAsync(newPolicy);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Policy has been created successfuly",
                Result = $"Policy Number : {newPolicy.PolicyNumber}"
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
    public async Task<BaseResponse> UpdateAsync(PolicyDto policyDto)
    {
        try
        {
            var validator = new PolicyValidator();
            var validatorResult = await validator.ValidateAsync(policyDto);

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


            var policy = await _context.Policies.FindAsync(policyDto.Id);
            if (policy == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Policy was not found",
                    Result = null
                };

            var userResponse = await _httpClient.GetAsync($"http://userservice:80/user/{policyDto.UserId}");
            var user = await userResponse.Content.ReadAsStringAsync();

            if (user == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User with this Id is not existed",
                    Result = null
                };

            var existedPolicyType = await _context.PolicyTypes
                .AnyAsync(w => w.Id == policyDto.PolicyTypeId);
            if (existedPolicyType == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This Policy Type is not existed",
                    Result = null
                };

            _mapper.Map(policyDto, policy);

            policy.UpdatedAt = DateTime.Now;

            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Policy has been updated successfully",
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
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Policy was not found",
                    Result = null
                };

            _context.Remove(policy);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Policy has been deleted successfully",
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
