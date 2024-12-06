using Bogus;
using Newtonsoft.Json;
using PaymentService.Entities;
using PaymentService.Utilities;
using PaymentService.Models.DTOs;
using PaymentService.Responses;

namespace PaymentService.Data;

public class SeedData
{
    private readonly PaymentContext _context;
    private readonly HttpClient _httpClient;
    private readonly TransactionNumberGenerator _transactionNumberGenerator;
    public SeedData(PaymentContext context, IHttpClientFactory httpClientFactory,
        TransactionNumberGenerator transactionNumberGenerator)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("ExternalService");
        _transactionNumberGenerator = transactionNumberGenerator;

    }

    #region SeedPaymentMethodsAsync
    public async Task SeedPaymentMethodsAsync()
    {
        if (_context.PaymentMethods.Any() == false)
        {
            var paymentMethodNames = new List<string> { "Cash", "Credit Card", "Bitcoin", "Tether" };


            var faker = new Faker();

            var paymentMethods = paymentMethodNames.Select(paymentMethods => new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = paymentMethods,
                Description = faker.Lorem.Sentence()
            }).ToList();

            await _context.PaymentMethods.AddRangeAsync(paymentMethods);
            await _context.SaveChangesAsync();
        }
    }
    #endregion

    #region SeedPaymentsAsync
    public async Task SeedPaymentsAsync()
    {
        if( _context.Payments.Any() == false)
        {
            var userResponse = await _httpClient.GetAsync("http://userservice:80/user");
            var userData = await userResponse.Content.ReadAsStringAsync();
            var userResponseObject = JsonConvert.DeserializeObject<BaseResponse<List<UserDto>>>(userData);
            var userIds = userResponseObject?.Result?.FirstOrDefault()?.Id;

            var paymentMethods = _context.PaymentMethods.ToList();

            var faker = new Faker<Payment>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.UserId, f => f.PickRandom(userIds))
                .RuleFor(p => p.Amount, f => f.Random.Decimal(300, 5000))
                .RuleFor(p => p.PaymentDate, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(p => p.Status, f => f.PickRandom<PaymentStatus>())
                .RuleFor(p => p.TransactionNumber, _transactionNumberGenerator.GenerateTransactionNumber())
                .RuleFor(p => p.PaymentMethodId, f => f.PickRandom(paymentMethods).Id)
                .RuleFor(p => p.Description, f => f.Lorem.Sentence());

            var payments = faker.Generate(20);

            await _context.Payments.AddRangeAsync(payments);
            await _context.SaveChangesAsync();
        }
    }
    #endregion

    #region SeedRefundsAsync 

    public async Task SeedRefundsAsync()
    {
        if (_context.Refunds.Any() == false)
        {
            var payments = _context.Payments.ToList();

            var faker = new Faker<Refund>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.PaymentId, f => f.PickRandom(payments).Id)
                .RuleFor(r => r.Amount, f => f.Random.Decimal(300, payments.FirstOrDefault().Amount))
                .RuleFor(r => r.RefundDate, f => f.Date.Past(1, DateTime.Now))
                .RuleFor(r => r.Status, f => f.PickRandom<RefundStatus>())
                .RuleFor(r => r.Reason, f => f.Lorem.Sentence());

            var refunds = faker.Generate(5);

            await _context.Refunds.AddRangeAsync(refunds);
            await _context.SaveChangesAsync();
        }
    }

    #endregion


}
