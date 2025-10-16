using FluentValidation;

namespace School.Application.Features.Students.Commands.Delete
{
    public class DeleteStudentCommandValidator: AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
