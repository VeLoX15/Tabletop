using FluentValidation;
using System.Globalization;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validators
{
    public class AbilityValidator : AbstractValidator<Ability>
    {
        public AbilityValidator()
        {
            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Description)
                .MaximumLength(1000)
                .WithMessage("Description must contain only 1000 characters.");

            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Mechanic)
                .MaximumLength(1000)
                .WithMessage("Mechanic must contain only 1000 characters.");
        }
    }
}