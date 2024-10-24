using Microsoft.AspNetCore.Mvc;
using PolicyService.Models.DTOs;
using PolicyService.Responses;
using PolicyService.Services.Implementations;
using PolicyService.Services.Interfaces;

namespace PolicyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PolicyTypeController(IPolicyTypeRepository policyTypeRepository) : ControllerBase
    {
        private readonly IPolicyTypeRepository _policyTypeRepository = policyTypeRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _policyTypeRepository.GetAllAsync();
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
            var result = await _policyTypeRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(PolicyTypeDto policyTypeDto)
        {
            var result = await _policyTypeRepository.AddAsync(policyTypeDto);
            return Created("", new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(PolicyTypeDto policyTypeDto)
        {
            var result = await _policyTypeRepository.UpdateAsync(policyTypeDto);
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
            var result = await _policyTypeRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

    }
}
