using MediatR;

namespace School.Application.Features.Students.Commands.Delete
{
    public record DeleteStudentCommand(int Id) : IRequest<bool>;
}
