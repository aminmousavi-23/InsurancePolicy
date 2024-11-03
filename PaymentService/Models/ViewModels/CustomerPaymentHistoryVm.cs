using PaymentService.Entities;

namespace PaymentService.Models.ViewModels;

public class CustomerPaymentHistoryVm
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PaymentId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
}
