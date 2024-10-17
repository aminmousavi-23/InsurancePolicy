namespace UserService.Entities
{
    public class UserLogin
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public DateTime LoginTime { get; set; } 
        public string IPAddress { get; set; } 
    }
}
