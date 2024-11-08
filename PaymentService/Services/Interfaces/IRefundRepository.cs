using PaymentService.Models.DTOs;
using PaymentService.Models.ViewModels;
using PaymentService.Responses;

namespace PaymentService.Services.Interfaces;

public interface IRefundRepository
{
    Task<BaseResponse<IList<RefundVm>>> GetAllAsync();
    Task<BaseResponse<RefundVm>> GetByIdAsync(Guid id);
    Task<BaseResponse> AddAsync(RefundDto refundDto);
    Task<BaseResponse> UpdateAsync(RefundDto refundDto);
    Task<BaseResponse> DeleteAsync(Guid id);
}
