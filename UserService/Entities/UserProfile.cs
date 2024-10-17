﻿namespace UserService.Entities
{
    public class UserProfile
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
