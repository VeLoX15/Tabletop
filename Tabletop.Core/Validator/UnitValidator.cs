using FluentValidation;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validator
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        public UnitValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("")
                .MaximumLength(20)
                .WithMessage("Name must contain only 20 characters.");

            RuleFor(x => x.Fraction)
                .NotEmpty()
                .WithMessage("")
                .MaximumLength(20)
                .WithMessage("Fraction must contain only 20 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("")
                .MaximumLength(255)
                .WithMessage("Description must contain only 255 characters.");

            RuleFor(x => x.Defense)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(11)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Defense value must be between 1 and 10");

            RuleFor(x => x.Moving)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(30)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Moving value must be between 1 and 30");
        }
    }
}