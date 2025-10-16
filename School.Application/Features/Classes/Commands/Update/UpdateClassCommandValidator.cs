using FluentValidation;

namespace School.Application.Features.Classes.Commands.Update
{
    public class UpdateClassCommandValidator: AbstractValidator<UpdateClassCommand>
    {
        public UpdateClassCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Class name is required")
                .MaximumLength(100).WithMessage("Class name must not exceed 100 characters")
                .MinimumLength(3).WithMessage("Class name must be at least 3 characters");

            RuleFor(x => x.TeacherName)
                .NotEmpty().WithMessage("Teacher name is required")
                .MaximumLength(100).WithMessage("Teacher name must not exceed 100 characters")
                .MinimumLength(2).WithMessage("Teacher name must be at least 2 characters");
        }
    }
}
