using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Entities;
using PaymentService.Utilities;
using PaymentService.Models.DTOs;
using PaymentService.Models.DTOs.Validators;
using PaymentService.Models.ViewModels;
using PaymentService.Responses;
using PaymentService.Services.Interfaces;
using System.Net.Http;

namespace PaymentService.Services.Implementations;

public class PaymentRepository(PaymentContext context, IMapper mapper, IHttpClientFactory httpClientFactory) 
    : IPaymentRepository
{
    private readonly PaymentContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("ExternalService");

    #region GetAllAsync
    public async Task<BaseResponse<IList<PaymentVm>>> GetAllAsync()
    {
        try
        {
            var payments = await _context.Payments
                .Include(p => p.PaymentMethod)
                .ToListAsync();
            return new BaseResponse<IList<PaymentVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<PaymentVm>>(payments)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<PaymentVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<PaymentVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
                return new BaseResponse<PaymentVm>
                {
                    IsSuccess = false,
                    Message = "There is no Payment with this id",
                    Result = null
                };

            return new BaseResponse<PaymentVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<PaymentVm>(payment)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<PaymentVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByTransactionNumberAsync
    public async Task<BaseResponse<PaymentVm>> GetByTransactionNumberAsync(string transactionNumber)
    {
        try
        {
            var payment = await _context.Payments
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(w => w.TransactionNumber == transactionNumber);
            if (payment == null)
                return new BaseResponse<PaymentVm>
                {
                    IsSuccess = false,
                    Message = "There is no Payment with this transaction number",
                    Result = null
                };

            return new BaseResponse<PaymentVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<PaymentVm>(payment)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<PaymentVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(PaymentDto paymentDto)
    {
        try
        {
            var validator = new PaymentValidator();
            var validatorResult = await validator.ValidateAsync(paymentDto);
            
            if (validatorResult.Errors.Any())
            {
                var errors = new List<string>();
                foreach (var error in validatorResult.Errors)
                    errors.Add(error.ErrorMessage);
            
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "BadRequest",
                    Result = errors
                };
            }

            var userResponse = await _httpClient.GetAsync($"http://userservice:80/user/{paymentDto.UserId}");
            var user = await userResponse.Content.ReadAsStringAsync();

            if (user == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User with this Id is not existed",
                    Result = null
                };

            var paymentMethod = await _context.PaymentMethods
                .FirstOrDefaultAsync(w => w.Id == paymentDto.PaymentMethodId);
            if (paymentMethod == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "The choosen payment method is not acceptable",
                    Result = null
                };

            var newPayment = _mapper.Map<Payment>(paymentDto);

            var transactionNumberGenerator = new TransactionNumberGenerator();
            newPayment.TransactionNumber = transactionNumberGenerator.GenerateTransactionNumber();

            await _context.AddAsync(newPayment);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "New Payment has been created successfully",
                Result = $"Payment Id : {newPayment.Id}"
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion
}
