using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PolicyService.Entities;

namespace PolicyService.Data
{
    public class PolicyContext : DbContext
    {
        public PolicyContext(DbContextOptions<PolicyContext> options) : base(options)
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
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyType> PolicyTypes { get; set; }
        public DbSet<Coverage> Coverages { get; set; }
        public DbSet<PolicyHistory> PolicyHistories { get; set; }
        public DbSet<Claim> Claims { get; set; }
    }
}
