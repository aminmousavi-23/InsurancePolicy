using PaymentService.Entities;

namespace PaymentService.Entities;

public class CustomerPaymentHistory //TODO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } 
    public Guid PaymentId { get; set; } 
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; } 
    public PaymentStatus Status { get; set; }
}
