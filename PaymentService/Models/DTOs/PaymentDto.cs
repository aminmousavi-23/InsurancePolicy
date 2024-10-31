using PaymentService.Entities;

namespace PaymentService.Models.DTOs;

public class PaymentDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentStatus Status { get; set; }
    public Guid PaymentMethodId { get; set; }
    public string Description { get; set; }
}
