namespace PaymentService.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; } 
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public string TransactionNumber { get; set; } 
    public PaymentMethod PaymentMethod { get; set; }
    public string Description { get; set; }
}
