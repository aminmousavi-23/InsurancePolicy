using Bogus;
using UserService.Entities;

namespace UserService.Data;

public class SeedData
{
    private readonly UserContext _context;
    public SeedData(UserContext context)
    {
        _context = context;
    }

    public async Task SeedUsersAsync()
    {
        if (_context.Users.Any() == false)
        {
            var roles = _context.Roles.ToList();

            var faker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.HashedPassword, f => f.Internet.Password(8))
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.RoleId, f => f.PickRandom(roles).Id)
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
                .RuleFor(u => u.UpdatedAt, f => f.Date.Recent(1));

            var users = faker.Generate(20);

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedRolesAsync()
    {
        if (_context.Roles.Any() == false)
        {
            var roleNames = new List<string> { "Admin", "Agent", "Customer" };

            var roles = roleNames.Select(roleName => new Role
            {
                Id = Guid.NewGuid(),
                RoleName = roleName
            }).ToList();

            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SeedUserProfilesAsync()
    {
        if (_context.UserProfiles.Any() == false)
        {
            var users = _context.Users.ToList();

            var faker = new Faker<UserProfile>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.UserId, f => f.PickRandom(users).Id)
                .RuleFor(up => up.Address, f => f.Address.StreetAddress())
                .RuleFor(up => up.City, f => f.Address.City())
                .RuleFor(up => up.State, f => f.Address.State())
                .RuleFor(up => up.Country, f => f.Address.Country())
                .RuleFor(up => up.PostalCode, f => f.Address.ZipCode());

            var userProfiles = users.Select(user => new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Address = faker.Generate().Address,
                City = faker.Generate().City,
                State = faker.Generate().State,
                Country = faker.Generate().Country,
                PostalCode = faker.Generate().PostalCode
            }).ToList();

            await _context.UserProfiles.AddRangeAsync(userProfiles);
            await _context.SaveChangesAsync();
        }
    }

}