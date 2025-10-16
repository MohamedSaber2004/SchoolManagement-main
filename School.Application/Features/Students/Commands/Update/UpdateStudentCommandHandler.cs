using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Students.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Students.Commands.Update
{
    public class UpdateStudentCommandHandler(IServiceManager _serviceManager,
                                      IValidator<UpdateStudentCommand> _validator,
                                      IMapper _mapper) : IRequestHandler<UpdateStudentCommand, StudentDto>
    {
        public async Task<StudentDto> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var updateStudent = _mapper.Map<UpdateStudentDto>(request);
            return await _serviceManager.StudentService.UpdateAsync(updateStudent);
        }
    }
}
