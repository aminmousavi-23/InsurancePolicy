namespace UserService.Entities
{
    public class User
    {
        public Guid Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string HashedPassword { get; set; }
        public string PhoneNumber { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public UserRole Role { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
