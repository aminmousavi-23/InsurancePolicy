using PolicyService.Entities;

namespace PolicyService.Models.DTOs
{
    public class PolicyDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PolicyTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public PolicyStatus Status { get; set; }
    }
}
