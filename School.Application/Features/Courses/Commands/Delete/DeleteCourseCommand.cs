using MediatR;

namespace School.Application.Features.Courses.Commands.Delete
{
    public record DeleteCourseCommand(int Id):IRequest<bool>;
}
