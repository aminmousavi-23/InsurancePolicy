using PaymentService.Entities;

namespace PaymentService.Entities;

public class CustomerPaymentHistory
{
    public Guid Id { get; set; } 
    public Guid CustomerId { get; set; } 
    public Guid PaymentId { get; set; } 
    public DateTime PaymentDate { get; set; } 
    public decimal Amount { get; set; } 
    public PaymentStatus Status { get; set; }
}
