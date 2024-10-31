using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Entities;
using PaymentService.Models.DTOs;
using PaymentService.Models.DTOs.Validators;
using PaymentService.Models.ViewModels;
using PaymentService.Responses;
using PaymentService.Services.Interfaces;

namespace PaymentService.Services.Implementations;

public class PaymentMethodRepository(PaymentContext context, IMapper mapper) : IPaymentMethodRepository
{
    private readonly PaymentContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region GetAllAsync
    public async Task<BaseResponse<IList<PaymentMethodVm>>> GetAllAsync()
    {
        try
        {
            var paymentMethods = await _context.PaymentMethods.ToListAsync();
            return new BaseResponse<IList<PaymentMethodVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<PaymentMethodVm>>(paymentMethods)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<PaymentMethodVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<PaymentMethodVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var paymentMethod = await _context.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
                return new BaseResponse<PaymentMethodVm>
                {
                    IsSuccess = false,
                    Message = "There is no Payment Method with this id",
                    Result = null
                };

            return new BaseResponse<PaymentMethodVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<PaymentMethodVm>(paymentMethod)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<PaymentMethodVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(PaymentMethodDto paymentMethodDto)
    {
        try
        {
            var validator = new PaymentMethodValidator();
            var validatorResult = await validator.ValidateAsync(paymentMethodDto);

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

            var existedPaymentMethod = await _context.PaymentMethods
            .AnyAsync(w => w.Name == paymentMethodDto.Name);
            if (existedPaymentMethod)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This Payment Method already existed",
                    Result = null
                };

            var newPaymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);

            newPaymentMethod.Id = Guid.NewGuid();

            await _context.AddAsync(newPaymentMethod);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Payment Method has been added successfully",
                Result = $"Payment Method Id : {newPaymentMethod.Id}"
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

    #region UpdateAsync
    public async Task<BaseResponse> UpdateAsync(PaymentMethodDto paymentMethodDto)
    {
        try
        {
            var validator = new PaymentMethodValidator();
            var validatorResult = await validator.ValidateAsync(paymentMethodDto);

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

            var paymentMethod = await _context.PaymentMethods
            .FindAsync(paymentMethodDto.Id);
            if (paymentMethod == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Payment Method was not found",
                    Result = null
                };

            _mapper.Map(paymentMethodDto, paymentMethod);

            _context.PaymentMethods.Update(paymentMethod);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Payment Method has been updated successfully",
                Result = null
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

    #region DeleteAsync
    public async Task<BaseResponse> DeleteAsync(Guid id)
    {
        try
        {
            var paymentMethod = await _context.PaymentMethods
            .FindAsync(id);
            if (paymentMethod == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Payment Method was not found",
                    Result = null
                };

            _context.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "PaymentMethod has been deleted successfully",
                Result = null
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
