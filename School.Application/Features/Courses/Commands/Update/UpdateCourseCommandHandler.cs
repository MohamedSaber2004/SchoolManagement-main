using AutoMapper;
using FluentValidation;
using MediatR;
using School.Application.Common.Exceptions;
using School.Application.Features.Courses.DTOs;
using School.Application.Interfaces.Services;

namespace School.Application.Features.Courses.Commands.Update
{
    public class UpdateCourseCommandHandler(IServiceManager _serviceManager,
                                            IValidator<UpdateCourseCommand> _validator,
                                            IMapper _mapper) : IRequestHandler<UpdateCourseCommand, CourseDto>
    {
        public async Task<CourseDto> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                throw new BadRequestException(errors);
            }

            var course = _mapper.Map<UpdateCourseCommand,UpdateCourseDto>(request);
            return await _serviceManager.CourseService.UpdateAsync(course);
        }
    }
}
