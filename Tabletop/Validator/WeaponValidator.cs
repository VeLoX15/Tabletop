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
                .WithMessage("")
                .MaximumLength(20)
                .WithMessage("Name must contain only 20 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("")
                .MaximumLength(255)
                .WithMessage("Description must contain only 255 characters.");

            RuleFor(x => x.Attack)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(11)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Attack value must be between 1 and 10");

            RuleFor(x => x.Quality)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(11)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Defense value must be between 1 and 10");

            RuleFor(x => x.Range)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(120)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Range value must be between 1 and 120");

            RuleFor(x => x.Dices)
                .NotEmpty()
                .WithMessage("")
                .NotNull()
                .WithMessage("")
                .LessThan(11)
                .WithMessage("")
                .GreaterThan(0)
                .WithMessage("Dices must be between 1 and 10");
        }
    }
}