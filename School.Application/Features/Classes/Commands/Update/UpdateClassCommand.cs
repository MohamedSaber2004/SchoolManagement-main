using MediatR;
using School.Application.Features.Classes.DTOs;

namespace School.Application.Features.Classes.Commands.Update
{
    public record UpdateClassCommand(int Id, string Name, string TeacherName): IRequest<ClassDto>;
}
