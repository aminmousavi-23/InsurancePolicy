namespace PaymentService.Features
{
    public class TransactionNumberGenerator
    {
        public string GenerateTransactionNumber()
        {
            return "Trs-" + Guid.NewGuid().ToString();
        }
    }
}
