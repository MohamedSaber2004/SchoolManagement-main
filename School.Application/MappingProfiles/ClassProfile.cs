using AutoMapper;
using School.Application.Features.Classes.Commands.Create;
using School.Application.Features.Classes.Commands.Update;
using School.Application.Features.Classes.DTOs;
using School.Domain.Entities;

namespace School.Application.MappingProfiles
{
    public class ClassProfile: Profile
    {
        public ClassProfile()
        {
            CreateMap<Class, ClassDto>();
            CreateMap<CreateClassCommand, CreateClassDto>();
            CreateMap<CreateClassDto, Class>();
            CreateMap<UpdateClassCommand, UpdateClassDto>();
            CreateMap<UpdateClassDto, Class>();
        }
    }
}
