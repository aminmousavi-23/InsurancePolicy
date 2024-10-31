using AutoMapper;
using PaymentService.Entities;
using PaymentService.Models.DTOs;
using PaymentService.Models.ViewModels;

namespace PaymentService.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Payment, PaymentDto>().ReverseMap();
        CreateMap<Payment, PaymentVm>().ReverseMap();

        CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
        CreateMap<PaymentMethod, PaymentMethodVm>().ReverseMap();

    }
}
