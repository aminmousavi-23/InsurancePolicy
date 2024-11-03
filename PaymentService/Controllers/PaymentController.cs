using Microsoft.AspNetCore.Mvc;
using PaymentService.Models.DTOs;
using PaymentService.Responses;
using PaymentService.Services.Interfaces;

namespace PaymentService.Controllers;

[Route("[controller]")]
[ApiController]
public class PaymentController(IPaymentRepository paymentRepository) : ControllerBase
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _paymentRepository.GetAllAsync();
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
        var result = await _paymentRepository.GetByIdAsync(id);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpGet("transactionNumber/{transactionNumber}")]
    public async Task<IActionResult> Get(string transactionNumber)
    {
        var result = await _paymentRepository.GetByTransactionNumberAsync(transactionNumber);
        return Ok(new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }

    [HttpPost]
    public async Task<IActionResult> Post(PaymentDto paymentDto)
    {
        var result = await _paymentRepository.AddAsync(paymentDto);
        return Created("", new BaseResponse
        {
            IsSuccess = result.IsSuccess,
            Message = result.Message,
            Result = result.Result
        });
    }
}
