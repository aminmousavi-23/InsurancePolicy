using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Profiles;
using UserService.Services.Implementations;
using UserService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

#region Database
string databaseHost = Environment.GetEnvironmentVariable("DB_HOST");
string databaseName = Environment.GetEnvironmentVariable("DB_NAME");
string databaseUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
string databasePassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
string databaseConnectionString = $"Data Source={databaseHost};Initial Catalog={databaseName};User ID={databaseUsername};Password={databasePassword};TrustServerCertificate=True;";

builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(databaseConnectionString);
});
#endregion 

#region Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
