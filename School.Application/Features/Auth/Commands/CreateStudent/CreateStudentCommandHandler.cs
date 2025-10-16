using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Auth.DTOs;
using School.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Features.Auth.Commands.CreateStudent
{
    internal class CreateStudentCommandHandler(IServiceManager _serviceManager,
                                               IValidator<CreateStudentCommand> _validator) : IRequestHandler<CreateStudentCommand, StudentAccountDto>
    {
        public Task<StudentAccountDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            return _serviceManager.AuthenticationService.CreateStudentAccountAsync(request.CreateStudentAccountDto, request.AdminUserId);
        }
    }
}
