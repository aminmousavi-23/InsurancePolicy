using Microsoft.AspNetCore.Mvc;
using PaymentService.Models.DTOs;
using PaymentService.Responses;
using PaymentService.Services.Interfaces;

namespace PaymentService.Controllers;

[Route("[controller]")]
[ApiController]
public class PaymentMethodController(IPaymentMethodRepository paymentMethodRepository) : ControllerBase
{
    private readonly IPaymentMethodRepository _paymentMethodRepository = paymentMethodRepository;
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _paymentMethodRepository.GetAllAsync();
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
        var result = await _paymentMethodRepository.GetByIdAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(PaymentMethodDto paymentMethodDto)
    {
        var result = await _paymentMethodRepository.AddAsync(paymentMethodDto);
        return Created("", new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPut]
    public async Task<IActionResult> Update(PaymentMethodDto paymentMethodDto)
    {
        var result = await _paymentMethodRepository.UpdateAsync(paymentMethodDto);
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
        var result = await _paymentMethodRepository.DeleteAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }
}
