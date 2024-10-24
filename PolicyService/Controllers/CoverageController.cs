using Microsoft.AspNetCore.Mvc;
using PolicyService.Models.DTOs;
using PolicyService.Responses;
using PolicyService.Services.Implementations;
using PolicyService.Services.Interfaces;

namespace PolicyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CoverageController(ICoverageRepository coverageRepository) : ControllerBase
    {
        private readonly ICoverageRepository _coverageRepository = coverageRepository;
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _coverageRepository.GetAllAsync();
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpGet("id")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _coverageRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(CoverageDto coverageDto)
        {
            var result = await _coverageRepository.AddAsync(coverageDto);
            return Created("", new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(CoverageDto coverageDto)
        {
            var result = await _coverageRepository.UpdateAsync(coverageDto);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _coverageRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }


    }
}
