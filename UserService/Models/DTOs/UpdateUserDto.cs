namespace UserService.Models.DTOs;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public string Email { get; set; }
    public string CurrentPassword { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime UpdatedAt { get; set; }
}
