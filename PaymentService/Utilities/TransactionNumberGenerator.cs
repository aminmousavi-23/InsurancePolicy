namespace PaymentService.Utilities
{
    public class TransactionNumberGenerator
    {
        public string GenerateTransactionNumber()
        {
            return "Trs-" + Guid.NewGuid().ToString();
        }
    }
}
