namespace PaymentService.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public decimal Amount { get; set; } 
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string TransactionNumber { get; set; }
    public Guid PaymentMethodId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string Description { get; set; }
}
