using FluentValidation;

namespace School.Application.Features.Courses.Commands.Create
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
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
