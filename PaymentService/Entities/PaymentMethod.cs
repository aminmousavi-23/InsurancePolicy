namespace PaymentService.Entities;

public class PaymentMethod
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Description { get; set; }
}
