using AutoMapper;
using PolicyService.Entities;
using PolicyService.Models.DTOs;
using PolicyService.Models.ViewModels;

namespace PolicyService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Policy, PolicyDto>().ReverseMap();
            CreateMap<Policy, PolicyVm>().ReverseMap();

            CreateMap<Claim, ClaimDto>().ReverseMap();
            CreateMap<Claim, ClaimVm>().ReverseMap();

            CreateMap<PolicyType, PolicyTypeDto>().ReverseMap();
            CreateMap<PolicyType, PolicyTypeVm>().ReverseMap();
        }
    }
}
