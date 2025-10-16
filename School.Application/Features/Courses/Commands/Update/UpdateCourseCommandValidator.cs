using FluentValidation;

namespace School.Application.Features.Courses.Commands.Update
{
    public class UpdateCourseCommandValidator: AbstractValidator<UpdateCourseCommand>
    {
        public UpdateCourseCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MaximumLength(100).WithMessage("Course name must not exceed 100 characters")
                .MinimumLength(3).WithMessage("Course name must be at least 3 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Course description is required")
                .MaximumLength(255).WithMessage("Course description must not exceed 255 characters")
                .MinimumLength(10).WithMessage("Course description must be at least 10 characters");
        }
    }
}
