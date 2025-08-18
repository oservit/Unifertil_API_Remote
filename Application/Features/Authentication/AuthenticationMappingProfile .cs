using AutoMapper;
using Domain.Features.Authentication;

namespace Application.Features.Authentication.Mapping
{
    public class AuthenticationMappingProfile : Profile
    {
        public AuthenticationMappingProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();
        }
    }
}
