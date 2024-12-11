using AutoMapper;
using PolicyService.Data;
using PolicyService.Entities;
using PolicyService.Models.DTOs.Validators;
using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Services.Interfaces;
using PolicyService.Responses;
using Microsoft.EntityFrameworkCore;
using PolicyService.Utilities;

namespace PolicyService.Services.Implementations;

public class ClaimRepository(PolicyContext context, IMapper mapper) : IClaimRepository
{
    private readonly PolicyContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region GetAllAsync
    public async Task<BaseResponse<IList<ClaimVm>>> GetAllAsync()
    {
        try
        {
            var claims = await _context.Claims.ToListAsync();
            return new BaseResponse<IList<ClaimVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<ClaimVm>>(claims)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<ClaimVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByClaimNumberAsync
    public async Task<BaseResponse<ClaimVm>> GetByClaimNumberAsync(string claimNumber)
    {
        try
        {
            var claim = await _context.Claims
                .FirstOrDefaultAsync(w => w.ClaimNumber == claimNumber);
            if (claim == null)
                return new BaseResponse<ClaimVm>
                {
                    IsSuccess = false,
                    Message = "Claim was not found",
                    Result = null
                };
            return new BaseResponse<ClaimVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<ClaimVm>(claim)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ClaimVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<ClaimVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null)
                return new BaseResponse<ClaimVm>
                {
                    IsSuccess = false,
                    Message = "There is no Claim with this id",
                    Result = null
                };

            return new BaseResponse<ClaimVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<ClaimVm>(claim)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<ClaimVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(ClaimDto claimDto)
    {
        try
        {
            var validator = new ClaimValidator();
            var validatorResult = await validator.ValidateAsync(claimDto);
            
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

            var existedClaim = await _context.Claims
                .AnyAsync(w => w.PolicyId == claimDto.PolicyId);
            if (existedClaim)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "A Claim for this PolicyId already existed",
                    Result = null
                };

            var existedPolicy = await _context.Policies
            .AnyAsync(w => w.Id == claimDto.PolicyId);
            if (existedPolicy == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This PolicyId is not existed",
                    Result = null
                };

            var expiredPolicy = await _context.Policies
                .AnyAsync(w => w.EndDate <= claimDto.ClaimDate);
            if (expiredPolicy)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "The choosen Policy has been expired",
                    Result = null
                };

            var newClaim = _mapper.Map<Claim>(claimDto);

            var generateUniqueNumber = new GenerateUniqueNumber();
            newClaim.ClaimNumber = generateUniqueNumber.GenerateClaimNumber();

            await _context.AddAsync(newClaim);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Claim has been created successfully",
                Result = $"Claim Number : {newClaim.ClaimNumber}"
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
    public async Task<BaseResponse> UpdateAsync(ClaimDto claimDto)
    {
        try
        {
            //var validator = new CoverageValidator();
            //var validatorResult = await validator.ValidateAsync(coverageDto);
            //
            //if (validatorResult.Errors.Any())
            //{
            //    var errors = new List<string>();
            //    foreach (var error in validatorResult.Errors)
            //        errors.Add(error.ErrorMessage);
            //
            //    return new BaseResponse
            //    {
            //        IsSuccess = false,
            //        Message = "BadRequest",
            //        Result = errors
            //    };
            //}

            var claim = await _context.Claims
            .FindAsync(claimDto.Id);
            if (claim == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Claim was not found",
                    Result = null
                };

            var existedPolicy = await _context.Policies
            .AnyAsync(w => w.Id == claimDto.PolicyId);
            if (existedPolicy == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This PolicyId is not existed",
                    Result = null
                };

            _mapper.Map(claimDto, claim);

            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Claim has been updated successfully",
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
            var claim = await _context.Claims
            .FindAsync(id);
            if (claim == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Claim was not found",
                    Result = null
                };

            _context.Remove(claim);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Claim has been deleted successfully",
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
