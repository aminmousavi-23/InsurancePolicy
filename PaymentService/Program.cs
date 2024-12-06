using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Utilities;
using PaymentService.Profiles;
using PaymentService.Services.Implementations;
using PaymentService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Database
string databaseHost = Environment.GetEnvironmentVariable("DB_HOST");
string databaseName = Environment.GetEnvironmentVariable("DB_NAME");
string databaseUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
string databasePassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
string databaseConnectionString = $"Data Source={databaseHost};Initial Catalog={databaseName};User ID={databaseUsername};Password={databasePassword};TrustServerCertificate=True;";

builder.Services.AddDbContext<PaymentContext>(options =>
{
    options.UseSqlServer(databaseConnectionString);
});
#endregion

#region Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient("ExternalService", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<TransactionNumberGenerator>();
builder.Services.AddTransient<SeedData>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<IRefundRepository, RefundRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<SeedData>();
    seeder.SeedPaymentMethodsAsync().GetAwaiter().GetResult();
    seeder.SeedPaymentsAsync().GetAwaiter().GetResult();
    seeder.SeedRefundsAsync().GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
