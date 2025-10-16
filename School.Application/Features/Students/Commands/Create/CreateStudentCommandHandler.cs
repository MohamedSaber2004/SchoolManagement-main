using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Students.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Students.Commands.Create
{
    public class CreateStudentCommandHandler(IServiceManager _serviceManager,
                                      IValidator<CreateStudentCommand> _validator,
                                      IMapper _mapper) : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var student = _mapper.Map<CreateStudentCommand,CreateStudentDto>(request);
            return await _serviceManager.StudentService.CreateAsync(student);
        }
    }
}
