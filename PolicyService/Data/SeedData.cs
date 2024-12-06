using Bogus;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PolicyService.Entities;
using PolicyService.Utilities;
using PolicyService.Models.DTOs;
using PolicyService.Responses;
using System.Net.Http;

namespace PolicyService.Data;

public class SeedData
{ 
    private readonly PolicyContext _context;
    private readonly GenerateUniqueNumber _uniqueNumber;
    private readonly HttpClient _httpClient;
    public SeedData(PolicyContext context, GenerateUniqueNumber uniqueNumber, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _uniqueNumber = uniqueNumber;
        _httpClient = httpClientFactory.CreateClient("ExternalService");
    }

    #region SeedPoliciesAsync
    public async Task SeedPoliciesAsync()
    {
        if(_context.Policies.Any() == false)
        {
            var userResponse = await _httpClient.GetAsync("http://userservice:80/user");
            var userData = await userResponse.Content.ReadAsStringAsync();
            var userResponseObject = JsonConvert.DeserializeObject<BaseResponse<List<UserDto>>>(userData);
            var userIds = userResponseObject?.Result?.FirstOrDefault()?.Id;

            var policyType = _context.PolicyTypes.ToList();

            var faker = new Faker<Policy>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.PolicyNumber, _uniqueNumber.GeneratePolicyNumber())
                .RuleFor(p => p.UserId, f => f.PickRandom(userIds))
                .RuleFor(p => p.PolicyTypeId, f => f.PickRandom(policyType).Id)
                .RuleFor(p => p.StartDate, f => f.Date.Past(5,DateTime.Now))
                .RuleFor(p => p.EndDate, f => f.Date.Future())
                .RuleFor(p => p.PremiumAmount, f => f.Random.Decimal(300, 5000))
                .RuleFor(p => p.Status, f => f.PickRandom<PolicyStatus>())
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
                .RuleFor(u => u.UpdatedAt, f => f.Date.Recent(1));

            var policies = faker.Generate(20);

            await _context.Policies.AddRangeAsync(policies);
            await _context.SaveChangesAsync();
        }
    }
    #endregion

    #region SeedPolicyTypesAsync
    public async Task SeedPolicyTypesAsync()
    {
        if (_context.PolicyTypes.Any() == false)
        {
            var policyTypeNames = new List<string> { "Car", "Motorcycle", "Phone", "Laptop"};


            var faker = new Faker();

            var policyTypes = policyTypeNames.Select(policyTypeNames => new PolicyType
            {
                Id = Guid.NewGuid(),
                Name = policyTypeNames,
                Description = faker.Lorem.Sentence()
            }).ToList();

            await _context.PolicyTypes.AddRangeAsync(policyTypes);
            await _context.SaveChangesAsync();
        }
    }
    #endregion

    #region SeedClaimAsync

    public async Task SeedClaimAsync()
    {
        if (_context.Claims.Any() == false)
        {
            var policies = _context.Policies.ToList();

            var faker = new Faker<Claim>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.PolicyId, f => f.PickRandom(policies).Id)
                .RuleFor(p => p.ClaimNumber, _uniqueNumber.GenerateClaimNumber())
                .RuleFor(p => p.ClaimDate, f =>
                {
                    var policy = f.PickRandom(policies);
                    return f.Date.Between(policy.StartDate.AddDays(1), policy.EndDate.AddDays(-1));
                })
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.ClaimAmount, f => f.Random.Decimal(300, 5000))
                .RuleFor(p => p.Status, f => f.PickRandom<ClaimStatus>());


            var claims = faker.Generate(20);
    
            await _context.Claims.AddRangeAsync(claims);
            await _context.SaveChangesAsync();
        }
    }

    #endregion

    #region SeedCoverageAsync

    public async Task SeedCoverageAsync()
    {
        if (_context.Coverages.Any() == false)
        {
            var policies = _context.Policies.ToList();
            var coverageNames = new List<string> { "Car Coverage", "Motorcycle Coverage",
                "Phone Coverage", "Laptop Coverage" };

            var faker = new Faker();

            var coverages = coverageNames.Select(coverageNames => new Coverage
            {
                Id = Guid.NewGuid(),
                PolicyId = faker.PickRandom(policies).Id,
                CoverageName = faker.PickRandom(coverageNames),
                Description = faker.Lorem.Sentence(),
                CoverageAmount = faker.Random.Decimal(600, 8000)
            }).ToList();

            await _context.Coverages.AddRangeAsync(coverages);
            await _context.SaveChangesAsync();
        }
    }

    #endregion
}
