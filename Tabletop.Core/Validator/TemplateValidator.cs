using FluentValidation;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validator
{
    public class TemplateValidator : AbstractValidator<Template>
    {
        public TemplateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.FractionId)
                .NotEmpty()
                .WithMessage("Faction must be selected");

            RuleFor(x => x.Force)
                .NotEmpty()
                .WithMessage("Force must be selected");

            RuleFor(x => x.TotalUsedForce)
                .LessThanOrEqualTo(x => x.Force)
                .WithMessage("Force points over limit");
        }
    }
}