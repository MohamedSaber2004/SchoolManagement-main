using AutoMapper;
using School.Application.Features.Auth.Commands.Register;
using School.Application.Features.Auth.DTOs;
using School.Application.Features.Auth.Queries.Login;

namespace School.Application.MappingProfiles
{
    public class AuthenticationProfile: Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<RegisterCommand, RegisterDto>();
            CreateMap<LoginQuery, LoginDto>();
        }
    }
}
