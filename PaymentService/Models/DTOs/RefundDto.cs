using PaymentService.Entities;

namespace PaymentService.Models.DTOs;

public class RefundDto
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime RefundDate { get; set; }
    public RefundStatus Status { get; set; }
    public string Reason { get; set; } = string.Empty;
}
