using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTOs;
using UserService.Responses;
using UserService.Services.Implementations;
using UserService.Services.Interfaces;

namespace UserService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProfileController(IUserProfileRepository userProfileRepository) : ControllerBase
    {
        private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userProfileRepository.GetAllAsync();
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
            var result = await _userProfileRepository.GetByIdAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserProfileDto userProfileDto)
        {
            var result = await _userProfileRepository.AddAsync(userProfileDto);
            return Created("",new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(UserProfileDto userProfileDto)
        {
            var result = await _userProfileRepository.UpdateAsync(userProfileDto);
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
            var result = await _userProfileRepository.DeleteAsync(id);
            return Ok(new BaseResponse
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Result = result.Result
            });
        }
    }
}
