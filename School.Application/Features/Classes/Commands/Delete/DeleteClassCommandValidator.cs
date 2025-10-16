using FluentValidation;

namespace School.Application.Features.Classes.Commands.Delete
{
    public class DeleteClassCommandValidator: AbstractValidator<DeleteClassCommand>
    {
        public DeleteClassCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
