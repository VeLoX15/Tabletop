using FluentValidation;
using System.Globalization;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validator
{
    public class WeaponValidator : AbstractValidator<Weapon>
    {
        public WeaponValidator()
        {
            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Description)
                .MaximumLength(500)
                .WithMessage("Description must contain only 500 characters.");

            RuleFor(x => x.Attack)
                .NotEmpty()
                .WithMessage("The field must be filled.")
                .LessThan(11)
                .WithMessage("The field may be a maximum of 10")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");

            RuleFor(x => x.Quality)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .LessThan(11)
                .WithMessage("The field may be a maximum of 10")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");

            RuleFor(x => x.Range)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .LessThan(150)
                .WithMessage("The field may be a maximum of 150")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");

            RuleFor(x => x.Dices)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .LessThan(11)
                .WithMessage("The field may be a maximum of 10")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");
        }
    }
}