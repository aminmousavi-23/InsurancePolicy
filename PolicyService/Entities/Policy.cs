namespace PolicyService.Entities;

public class Policy //TODO Id and CreateDate for all entities
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PolicyNumber { get; set; } 
    public Guid UserId { get; set; } 
    public Guid PolicyTypeId { get; set; } 
    public DateTime StartDate { get; set; } 
    public DateTime EndDate { get; set; } 
    public decimal PremiumAmount { get; set; } // مبلغ حق بیمه
    public PolicyStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }
}
