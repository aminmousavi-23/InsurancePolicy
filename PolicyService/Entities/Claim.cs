namespace PolicyService.Entities;

public class Claim
{
    public Guid Id { get; set; }
    public Guid PolicyId { get; set; } 
    public string ClaimNumber { get; set; } 
    public DateTime ClaimDate { get; set; } 
    public string Description { get; set; } 
    public decimal ClaimAmount { get; set; } 
    public ClaimStatus Status { get; set; } 
}
