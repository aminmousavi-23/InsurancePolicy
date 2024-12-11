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

public class RefundRepository(PaymentContext context, IMapper mapper) : IRefundRepository
{
    private readonly PaymentContext _context = context;
    private readonly IMapper _mapper = mapper;

    #region GetAllAsync
    public async Task<BaseResponse<IList<RefundVm>>> GetAllAsync()
    {
        try
        {
            var refunds = await _context.Refunds.ToListAsync();
            return new BaseResponse<IList<RefundVm>>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<IList<RefundVm>>(refunds)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<IList<RefundVm>>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region GetByIdAsync
    public async Task<BaseResponse<RefundVm>> GetByIdAsync(Guid id)
    {
        try
        {
            var refund = await _context.Refunds.FindAsync(id);
            if (refund == null)
                return new BaseResponse<RefundVm>
                {
                    IsSuccess = false,
                    Message = "There is no Refund request with this id",
                    Result = null
                };

            return new BaseResponse<RefundVm>
            {
                IsSuccess = true,
                Message = "",
                Result = _mapper.Map<RefundVm>(refund)
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<RefundVm>
            {
                IsSuccess = false,
                Message = ex.Message,
                Result = null
            };
        }
    }
    #endregion

    #region AddAsync
    public async Task<BaseResponse> AddAsync(RefundDto refundDto)
    {
        try
        {
            var validator = new RefundValidator();
            var validatorResult = await validator.ValidateAsync(refundDto);
            
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

            var existedRefund = await _context.Refunds
            .AnyAsync(w => w.PaymentId == refundDto.PaymentId);
            if (existedRefund)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "This Payment already has a refund request",
                    Result = null
                };

            var existedPayment = await _context.Payments
                .AnyAsync(w => w.Id == refundDto.PaymentId);
            if (existedPayment == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "There is no Payment with this Id",
                    Result = null
                };

            var paymentAmount = await _context.Payments
                .AnyAsync(w => w.Id == refundDto.PaymentId && w.Amount < refundDto.Amount);
            if (paymentAmount)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Refund amount can not be greater than payment amount",
                    Result = null
                };

            var newRefund = _mapper.Map<Refund>(refundDto);

            await _context.AddAsync(newRefund);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Refund request has been added successfully",
                Result = $"Refund request Id : {newRefund.Id}"
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
    public async Task<BaseResponse> UpdateAsync(RefundDto refundDto)
    {
        try
        {
            var validator = new RefundValidator();
            var validatorResult = await validator.ValidateAsync(refundDto);
            
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

            var refund = await _context.Refunds
                .FindAsync(refundDto.Id);
            if (refund == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "There is no Refund request with this Id",
                    Result = null
                };

            var existedPayment = await _context.Payments
                .AnyAsync(w => w.Id == refundDto.PaymentId);
            if (existedPayment == false)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "There is no Payment with this Id",
                    Result = null
                };

            _mapper.Map(refundDto, refund);

            _context.Refunds.Update(refund);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Refund request has been updated successfully",
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
            var refund = await _context.Refunds
            .FindAsync(id);
            if (refund == null)
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Refund request was not found",
                    Result = null
                };

            _context.Remove(refund);
            await _context.SaveChangesAsync();

            return new BaseResponse
            {
                IsSuccess = true,
                Message = "Refund request has been deleted successfully",
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
