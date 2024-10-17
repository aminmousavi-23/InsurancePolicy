namespace UserService.Entities
{
    public class UserToken
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; } 
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; } 
        public DateTime ExpireDate { get; set; } 
    }
}
