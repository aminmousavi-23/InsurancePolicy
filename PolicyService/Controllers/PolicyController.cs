using Microsoft.AspNetCore.Mvc;
using PolicyService.Models.DTOs;
using PolicyService.Responses;
using PolicyService.Services.Interfaces;

namespace PolicyService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PolicyController(IPolicyRepository policyRepository) : ControllerBase
    {
        private readonly IPolicyRepository _policyRepository = policyRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _policyRepository.GetAllAsync();
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _policyRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpGet("{PolicyNumber}")]
        public async Task<IActionResult> Get(string policyNumber)
        {
            var result = await _policyRepository.GetByPolicyNumberAsync(policyNumber);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(PolicyDto policyDto)
        {
            var result = await _policyRepository.AddAsync(policyDto);
            return Created("", new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(PolicyDto policyDto)
        {
            var result = await _policyRepository.UpdateAsync(policyDto);
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
            var result = await _policyRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

    }
}
