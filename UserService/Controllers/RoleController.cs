using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTOs;
using UserService.Responses;
using UserService.Services.Implementations;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController(IRoleRepository roleRepository) : ControllerBase
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _roleRepository.GetAllAsync();
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
            var result = await _roleRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(RoleDto roleDto)
        {
            var result = await _roleRepository.AddAsync(roleDto);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoleDto roleDto)
        {
            var result = await _roleRepository.UpdateAsync(roleDto);
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
            var result = await _roleRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

    }
}

