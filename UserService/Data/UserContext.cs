using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using UserService.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserService.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        try
        {
            var database = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (database != null)
            {
                if (!database.CanConnect()) database.Create();
                if (!database.HasTables()) database.CreateTables();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
}
