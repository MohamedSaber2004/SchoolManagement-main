using AutoMapper;
using School.Application.Features.Courses.Commands.Create;
using School.Application.Features.Courses.Commands.Update;
using School.Application.Features.Courses.DTOs;
using School.Domain.Entities;

namespace School.Application.MappingProfiles
{
    public class CourseProfile: Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
            CreateMap<CreateCourseCommand, CreateCourseDto>();
            CreateMap<UpdateCourseCommand, UpdateCourseDto>();
        }
    }
}
