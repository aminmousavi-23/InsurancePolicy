using Microsoft.AspNetCore.Mvc;
using PolicyService.Models.DTOs;
using PolicyService.Responses;
using PolicyService.Services.Implementations;
using PolicyService.Services.Interfaces;

namespace PolicyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClaimController(IClaimRepository claimRepository) : ControllerBase
    {
        private readonly IClaimRepository _claimRepository = claimRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _claimRepository.GetAllAsync();
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
            var result = await _claimRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpGet("ClaimNumber")]
        public async Task<IActionResult> Get(string claimNumber)
        {
            var result = await _claimRepository.GetByClaimNumberAsync(claimNumber);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClaimDto claimDto)
        {
            var result = await _claimRepository.AddAsync(claimDto);
            return Created("", new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(ClaimDto claimDto)
        {
            var result = await _claimRepository.UpdateAsync(claimDto);
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
            var result = await _claimRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }
    }
}
