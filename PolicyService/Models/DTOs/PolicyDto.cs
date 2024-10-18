using PolicyService.Entities;

namespace PolicyService.Models.DTOs
{
    public class PolicyDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid PolicyTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; } // مبلغ حق بیمه
        public PolicyStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
