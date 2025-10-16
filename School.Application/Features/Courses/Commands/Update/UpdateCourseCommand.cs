using MediatR;
using School.Application.Features.Courses.DTOs;

namespace School.Application.Features.Courses.Commands.Update
{
    public record UpdateCourseCommand(int Id, string Name, string Description) : IRequest<CourseDto>;
}
