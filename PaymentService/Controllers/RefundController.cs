using Microsoft.AspNetCore.Mvc;
using PaymentService.Models.DTOs;
using PaymentService.Responses;
using PaymentService.Services.Interfaces;

namespace PaymentService.Controllers;

[Route("[controller]")]
[ApiController]
public class RefundController(IRefundRepository refundRepository) : ControllerBase
{
    private readonly IRefundRepository _refundRepository = refundRepository;
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _refundRepository.GetAllAsync();
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
        var result = await _refundRepository.GetByIdAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(RefundDto refundDto)
    {
        var result = await _refundRepository.AddAsync(refundDto);
        return Created("", new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(RefundDto refundDto)
    {
        var result = await _refundRepository.UpdateAsync(refundDto);
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
        var result = await _refundRepository.DeleteAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }
}
