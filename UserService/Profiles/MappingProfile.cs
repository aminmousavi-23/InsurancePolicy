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
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, UserVm>().ReverseMap();

            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, RoleVm>().ReverseMap();

            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileVm>().ReverseMap();

            CreateMap<UserRole, UserRoleDto>().ReverseMap();
            CreateMap<UserRole, UserRoleVm>().ReverseMap();
        }
    }
}
