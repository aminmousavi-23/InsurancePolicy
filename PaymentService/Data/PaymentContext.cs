using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PaymentService.Entities;

namespace PaymentService.Data;

public class PaymentContext : DbContext
{
    public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
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

    public DbSet<Payment> Payments { get; set; }
    public DbSet<Refund> Refunds { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<CustomerPaymentHistory> CustomerPaymentHistories { get; set; }
}
