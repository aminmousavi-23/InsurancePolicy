using PolicyService.Entities;

namespace PolicyService.Models.DTOs
{
    public class ClaimDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public DateTime ClaimDate { get; set; }
        public string Description { get; set; }
        public decimal ClaimAmount { get; set; }
        public ClaimStatus Status { get; set; }
    }
}
