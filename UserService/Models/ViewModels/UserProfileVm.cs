namespace UserService.Models.ViewModels
{
    public class UserProfileVm
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
