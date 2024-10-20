﻿namespace PolicyService.Entities
{
    public class Coverage
    {
        public Guid Id { get; set; } 
        public Guid PolicyId { get; set; } 
        public string CoverageName { get; set; }
        public string Description { get; set; } 
        public decimal CoverageAmount { get; set; } 
    }

}