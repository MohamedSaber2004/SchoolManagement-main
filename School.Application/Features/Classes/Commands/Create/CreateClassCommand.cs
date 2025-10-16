using MediatR;
using School.Application.Features.Classes.DTOs;

namespace School.Application.Features.Classes.Commands.Create
{
    public record CreateClassCommand(string Name, string TeacherName): IRequest<ClassDto>;
}
