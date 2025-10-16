using MediatR;
using School.Application.Features.Courses.DTOs;

namespace School.Application.Features.Courses.Commands.Create
{
    public record CreateCourseCommand(string Name, string Description): IRequest<CourseDto>;
}
