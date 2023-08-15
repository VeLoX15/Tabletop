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
                .WithMessage("Attack must be filled.");

            RuleFor(x => x.Quality)
                .NotEmpty()
                .WithMessage("Quality must be filled");

            RuleFor(x => x.Range)
                .NotEmpty()
                .WithMessage("Range must be filled");

            RuleFor(x => x.Dices)
                .NotEmpty()
                .WithMessage("Dices must be filled");
        }
    }
}