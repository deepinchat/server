using AutoMapper;
using Deepin.Application.DTOs.Users;
using Deepin.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Deepin.Application.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>(MemberList.Destination);
        CreateMap<IdentityUserClaim<Guid>, UserCliamDto>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ClaimType, opt => opt.MapFrom(src => src.ClaimType))
            .ForMember(dest => dest.ClaimValue, opt => opt.MapFrom(src => src.ClaimValue));
    }
}
