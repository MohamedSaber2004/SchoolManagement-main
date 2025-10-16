using MediatR;
using School.Application.Features.Auth.DTOs;

namespace School.Application.Features.Auth.Commands.CreateStudent
{
    public record CreateStudentCommand(CreateStudentAccountDto CreateStudentAccountDto,string AdminUserId) : IRequest<StudentAccountDto>;
}
