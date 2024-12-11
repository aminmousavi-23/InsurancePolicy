namespace PolicyService.Entities;

public class Coverage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PolicyId { get; set; } 
    public string CoverageName { get; set; }
    public string Description { get; set; } 
    public decimal CoverageAmount { get; set; } 
}
