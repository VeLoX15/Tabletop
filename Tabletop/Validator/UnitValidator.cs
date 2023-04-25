using FluentValidation;
using Tabletop.Models;

namespace Tabletop.Validator
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        public UnitValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Name must contain only 20 characters.");

            RuleFor(x => x.Fraction)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Fraction must contain only 20 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Description must contain only 255 characters.");

            RuleFor(x => x.Defense)
                .NotEmpty()
                .NotNull()
                .LessThan(11)
                .GreaterThan(0)
                .WithMessage("Defense value must be between 1 and 10");

            RuleFor(x => x.Moving)
                .NotEmpty()
                .NotNull()
                .LessThan(30)
                .GreaterThan(0)
                .WithMessage("Moving value must be between 1 and 30");
        }
    }
}