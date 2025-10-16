using AutoMapper;
using School.Application.Features.Students.Commands.Create;
using School.Application.Features.Students.Commands.Update;
using School.Application.Features.Students.DTOs;
using School.Domain.Entities;

namespace School.Application.MappingProfiles
{
    public class StudentProfile: Profile
    {
        public StudentProfile()
        {
            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.ClassName, options => options.MapFrom(src => src.Class!.Name));

            CreateMap<CreateStudentDto, Student>();
            CreateMap<UpdateStudentDto, Student>();

            CreateMap<CreateStudentCommand, CreateStudentDto>();
            CreateMap<UpdateStudentCommand, UpdateStudentDto>();

            CreateMap<StudentDto, CreateStudentDto>();
        }
    }
}
