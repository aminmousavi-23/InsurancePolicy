using Microsoft.AspNetCore.Mvc;
using UserService.Models.DTOs;
using UserService.Responses;
using UserService.Services.Interfaces;

namespace UserService.Controllers;

[Route("[controller]")]
[ApiController]
public class UserRoleController(IUserRoleRepository userRoleRepository) : ControllerBase
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _userRoleRepository.GetAllAsync();
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
        var result = await _userRoleRepository.GetByIdAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserRoleDto userRoleDto)
    {
        var result = await _userRoleRepository.AddAsync(userRoleDto);
        return Created("", new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserRoleDto userRoleDto)
    {
        var result = await _userRoleRepository.UpdateAsync(userRoleDto);
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
        var result = await _userRoleRepository.DeleteAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }
}
