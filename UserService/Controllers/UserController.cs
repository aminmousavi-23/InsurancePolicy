using Microsoft.AspNetCore.Mvc;
using UserService.DTOs;
using UserService.Entities;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userRepository.GetAllAsync();
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
            var result = await _userRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var result = await _userRepository.AddAsync(userDto);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserDto userDto)
        {
            var result = await _userRepository.UpdateAsync(userDto);
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
            var result = await _userRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

    }
}
