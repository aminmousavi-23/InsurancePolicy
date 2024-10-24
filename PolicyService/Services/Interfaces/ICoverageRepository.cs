using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;
using PolicyService.Responses;

namespace PolicyService.Services.Interfaces;

public interface ICoverageRepository
{
    Task<BaseResponse<IList<CoverageVm>>> GetAllAsync();
    Task<BaseResponse<CoverageVm>> GetByIdAsync(Guid id);
    Task<BaseResponse> AddAsync(CoverageDto coverageDto);
    Task<BaseResponse> UpdateAsync(CoverageDto coverageDto);
    Task<BaseResponse> DeleteAsync(Guid id);
}
