using AutoMapper;
using UserService.Entities;
using UserService.Models.DTOs;
using UserService.Models.ViewModels;

namespace UserService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleVm>().ReverseMap();
        }
    }
}
