using FluentValidation;

namespace School.Application.Features.Students.Commands.Update
{
    public class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
    {
        public UpdateStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            RuleFor(x => x.ClassId)
                .GreaterThan(0).WithMessage("ClassId must be greater than 0")
                .When(x => x.ClassId.HasValue);
        }
    }
}
