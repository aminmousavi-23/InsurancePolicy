using PaymentService.Models.DTOs;
using PaymentService.Models.ViewModels;
using PaymentService.Responses;

namespace PaymentService.Services.Interfaces;

public interface IPaymentMethodRepository
{
    Task<BaseResponse<IList<PaymentMethodVm>>> GetAllAsync();
    Task<BaseResponse<PaymentMethodVm>> GetByIdAsync(Guid id);
    Task<BaseResponse> AddAsync(PaymentMethodDto paymentMethodDto);
    Task<BaseResponse> UpdateAsync(PaymentMethodDto paymentMethodDto);
    Task<BaseResponse> DeleteAsync(Guid id);
}
