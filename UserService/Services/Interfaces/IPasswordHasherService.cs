﻿namespace UserService.Services.Interfaces
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
    }

}
