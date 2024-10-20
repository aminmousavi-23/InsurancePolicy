namespace PolicyService.Features
{
    public class GenerateUniqueNumber
    {
        public string GenerateClaimNumber()
        {
            return "CLAIM-" + Guid.NewGuid().ToString();
        }


        public string GeneratePolicyNumber()
        {
            return "Policy-" + Guid.NewGuid().ToString();
        }
    }
}
