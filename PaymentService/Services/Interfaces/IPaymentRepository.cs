using PaymentService.Models.DTOs;
using PaymentService.Models.ViewModels;
using PaymentService.Responses;

namespace PaymentService.Services.Interfaces
{
    public interface IPaymentRepository
    {
        Task<BaseResponse<IList<PaymentVm>>> GetAllAsync();
        Task<BaseResponse<PaymentVm>> GetByIdAsync(Guid id);
        Task<BaseResponse<PaymentVm>> GetByTransactionNumberAsync(string transactionNumber);
        Task<BaseResponse> AddAsync(PaymentDto paymentDto);
    }
}
