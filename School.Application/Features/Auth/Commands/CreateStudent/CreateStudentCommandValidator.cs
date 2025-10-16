using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Features.Auth.Commands.CreateStudent
{
    public class CreateStudentCommandValidator: AbstractValidator<CreateStudentCommand>
    {
        public CreateStudentCommandValidator()
        {
            RuleFor(x => x.AdminUserId)
                .NotEmpty()
                .WithMessage("AdminUserId is required")
                .NotNull()
                .WithMessage("AdminUserId cannot be null");

            RuleFor(x => x.CreateStudentAccountDto)
                .NotNull()
                .WithMessage("Student account data is required");

            When(x => x.CreateStudentAccountDto != null, () =>
            {
                RuleFor(x => x.CreateStudentAccountDto.Name)
                    .NotEmpty()
                    .WithMessage("Student name is required")
                    .MinimumLength(2)
                    .WithMessage("Name must be at least 2 characters")
                    .MaximumLength(100)
                    .WithMessage("Name must not exceed 100 characters")
                    .Matches(@"^[a-zA-Z\s'-]+$")
                    .WithMessage("Name can only contain letters, spaces, hyphens, and apostrophes");

                RuleFor(x => x.CreateStudentAccountDto.Email)
                    .NotEmpty()
                    .WithMessage("Email is required")
                    .EmailAddress()
                    .WithMessage("Invalid email format")
                    .MaximumLength(100)
                    .WithMessage("Email must not exceed 100 characters");

                RuleFor(x => x.CreateStudentAccountDto.UserName)
                    .NotEmpty()
                    .WithMessage("Username is required")
                    .MinimumLength(3)
                    .WithMessage("Username must be at least 3 characters")
                    .MaximumLength(50)
                    .WithMessage("Username must not exceed 50 characters")
                    .Matches(@"^[a-zA-Z0-9_]+$")
                    .WithMessage("Username can only contain letters, numbers, and underscores");

                RuleFor(x => x.CreateStudentAccountDto.PhoneNumber)
                    .Matches(@"^\+?[1-9]\d{1,14}$")
                    .When(x => !string.IsNullOrEmpty(x.CreateStudentAccountDto.PhoneNumber))
                    .WithMessage("Invalid phone number format. Use international format (e.g., +201234567890)");

                RuleFor(x => x.CreateStudentAccountDto.ClassId)
                    .GreaterThan(0)
                    .When(x => x.CreateStudentAccountDto.ClassId.HasValue)
                    .WithMessage("ClassId must be a positive number");

                RuleFor(x => x.CreateStudentAccountDto.Password)
                    .MinimumLength(8)
                    .When(x => !string.IsNullOrEmpty(x.CreateStudentAccountDto.Password))
                    .WithMessage("Password must be at least 8 characters")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*()_+\-=\[\]{};':""\\|,.<>/?]).{8,}$")
                    .When(x => !string.IsNullOrEmpty(x.CreateStudentAccountDto.Password))
                    .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");
            });
        }
    }
}
