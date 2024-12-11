namespace PolicyService.Entities;

public class PolicyHistory  //TODO
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PolicyId { get; set; } 
    public string Action { get; set; } // نوع تغییر (ایجاد، ویرایش، لغو و غیره)
    public DateTime ActionDate { get; set; } = DateTime.Now;
    public string PerformedBy { get; set; } 
}
