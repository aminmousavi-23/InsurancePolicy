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
    }
}
