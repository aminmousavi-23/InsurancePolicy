namespace UserService.Utilities;

public interface IPasswordHasherService
{
    string HashPassword(string password);
    string VerifyHashedPassword(object updateUserDto, string hashedPassword, string currentPassword);
}


