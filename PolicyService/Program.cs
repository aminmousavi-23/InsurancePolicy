using Microsoft.EntityFrameworkCore;
using PolicyService.Data;
using PolicyService.Profiles;
using PolicyService.Services.Implementations;
using PolicyService.Services.Interfaces;
using PolicyService.Utilities;

var builder = WebApplication.CreateBuilder(args);

#region Database
string databaseHost = Environment.GetEnvironmentVariable("DB_HOST");
string databaseName = Environment.GetEnvironmentVariable("DB_NAME");
string databaseUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
string databasePassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
string databaseConnectionString = $"Data Source={databaseHost};Initial Catalog={databaseName};User ID={databaseUsername};Password={databasePassword};TrustServerCertificate=True;";

builder.Services.AddDbContext<PolicyContext>(options =>
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

builder.Services.AddScoped<GenerateUniqueNumber>();
builder.Services.AddTransient<SeedData>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPolicyTypeRepository, PolicyTypeRepository>();
builder.Services.AddScoped<ICoverageRepository, CoverageRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();


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
    seeder.SeedPolicyTypesAsync().GetAwaiter().GetResult();
    seeder.SeedPoliciesAsync().GetAwaiter().GetResult();
    seeder.SeedClaimAsync().GetAwaiter().GetResult();
    seeder.SeedCoverageAsync().GetAwaiter().GetResult();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
