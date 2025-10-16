using MediatR;
using School.Application.Features.Students.DTOs;

namespace School.Application.Features.Students.Queries.GetById
{
    public record GetStudentByIdQuery(int Id): IRequest<StudentDto>;
}
