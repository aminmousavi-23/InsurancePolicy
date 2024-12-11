using AutoMapper;
using PolicyService.Data;
using PolicyService.Entities;
using PolicyService.Models.DTOs.Validators;
using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Services.Interfaces;
using PolicyService.Responses;
using Microsoft.EntityFrameworkCore;

namespace PolicyService.Services.Implementations;

public class CoverageRepository(PolicyContext context, IMapper mapper) : ICoverageRepository
{
    private readonly PolicyContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region GetAllAsync
    public async Task<BaseResponse<IList<CoverageVm>>> GetAllAsync()
    {
        try
        {
            var coverages = await _context.Coverages.ToListAsync();
            return new BaseResponse<IList<CoverageVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<CoverageVm>>(coverages)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<CoverageVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<CoverageVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var coverage = await _context.Coverages.FindAsync(id);
            if (coverage == null)
                return new BaseResponse<CoverageVm>
                {
                    IsSuccess = false,
                    Message = "There is no Coverage with this id",
                    Result = null
                };

            return new BaseResponse<CoverageVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<CoverageVm>(coverage)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<CoverageVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(CoverageDto coverageDto)
    {
        try
        {
            var validator = new CoverageValidator();
            var validatorResult = await validator.ValidateAsync(coverageDto);
            
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

            var existedCoverage = await _context.Coverages
            .AnyAsync(w => w.CoverageName == coverageDto.CoverageName);
            if (existedCoverage)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This Coverage already existed",
                    Result = null
                };

            var existedPolicy = await _context.Policies
                .AnyAsync(w => w.Id ==  coverageDto.PolicyId);
            if (existedPolicy == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This PolicyId is not existed",
                    Result = null
                };

            var newCoverage = _mapper.Map<Coverage>(coverageDto);

            await _context.AddAsync(newCoverage);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Coverage has been created successfully",
                Result = $"Coverage Id : {newCoverage.Id}"
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
    public async Task<BaseResponse> UpdateAsync(CoverageDto coverageDto)
    {
        try
        {
            var validator = new CoverageValidator();
            var validatorResult = await validator.ValidateAsync(coverageDto);
            
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

            var coverage = await _context.Coverages
            .FindAsync(coverageDto.Id);
            if (coverage == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Coverage was not found",
                    Result = null
                };

            _mapper.Map(coverageDto, coverage);

            _context.Coverages.Update(coverage);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Coverage has been updated successfully",
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
            var coverage = await _context.Coverages
            .FindAsync(id);
            if (coverage == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Coverage was not found",
                    Result = null
                };

            _context.Remove(coverage);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Coverage has been deleted successfully",
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
