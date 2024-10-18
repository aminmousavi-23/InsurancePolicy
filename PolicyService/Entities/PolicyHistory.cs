namespace PolicyService.Entities
{
    public class PolicyHistory
    {
        public Guid Id { get; set; } 
        public Guid PolicyId { get; set; } 
        public string Action { get; set; } // نوع تغییر (ایجاد، ویرایش، لغو و غیره)
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; } 
    }

}
