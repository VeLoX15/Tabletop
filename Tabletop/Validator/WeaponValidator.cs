using FluentValidation;
using Tabletop.Models;

namespace Tabletop.Validator
{
    public class WeaponValidator : AbstractValidator<Weapon>
    {
        public WeaponValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("Name must contain only 20 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(255)
                .WithMessage("Name must contain only 255 characters.");

            RuleFor(x => x.Attack)
                .NotEmpty()
                .NotNull()
                .LessThan(11)
                .GreaterThan(0)
                .WithMessage("Attack value must be between 1 and 10");

            RuleFor(x => x.Quality)
                .NotEmpty()
                .NotNull()
                .LessThan(11)
                .GreaterThan(0)
                .WithMessage("Defense value must be between 1 and 10");

            RuleFor(x => x.Range)
                .NotEmpty()
                .NotNull()
                .LessThan(120)
                .GreaterThan(0)
                .WithMessage("Range value must be between 1 and 120");

            RuleFor(x => x.Dices)
                .NotEmpty()
                .NotNull()
                .LessThan(11)
                .GreaterThan(0)
                .WithMessage("Dices must be between 1 and 10");
        }
    }
}