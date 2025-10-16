using MediatR;
using School.Application.Features.Students.DTOs;

namespace School.Application.Features.Students.Commands.Update
{
    public record UpdateStudentCommand(int Id, string Name, string Email, int? ClassId): IRequest<StudentDto>;
}
