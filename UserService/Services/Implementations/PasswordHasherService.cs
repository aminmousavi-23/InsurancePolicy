using Microsoft.AspNetCore.Identity;
using UserService.Services.Interfaces;

namespace UserService.Services.Implementations
{
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }

        public string VerifyHashedPassword(object updateUserDto,string hashedPassword, string currentPassword)
        {
            var hasher = new PasswordHasher<object>();
            var verify =  hasher.VerifyHashedPassword(updateUserDto, hashedPassword, currentPassword);
            return verify.ToString();
        }
    }
}
