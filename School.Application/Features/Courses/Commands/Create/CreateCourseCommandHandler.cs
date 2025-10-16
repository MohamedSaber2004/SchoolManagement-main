using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Courses.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Courses.Commands.Create
{
    internal class CreateCourseCommandHandler(IServiceManager _serviceManager,
                                              IValidator<CreateCourseCommand> _validator,
                                              IMapper _mapper) : IRequestHandler<CreateCourseCommand, CourseDto>
    {
        public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var course = _mapper.Map<CreateCourseCommand, CreateCourseDto>(request);
            return await _serviceManager.CourseService.CreateAsync(course);
        }
    }
}
