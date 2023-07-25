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
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.FractionId)
                .NotEmpty()
                .WithMessage("Faction must be selected");

            RuleFor(x => x.Description)
                .MaximumLength(400)
                .WithMessage("Description must contain only 400 characters.");

            RuleFor(x => x.Mechanic)
                .MaximumLength(400)
                .WithMessage("Description must contain only 400 characters.");
            
            RuleFor(x => x.Defense)
                .NotEmpty()
                .WithMessage("The field must be filled.") 
                .LessThan(11)
                .WithMessage("The field may be a maximum of 10")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");

            RuleFor(x => x.Moving)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .LessThan(30)
                .WithMessage("The field may be a maximum of 30")
                .GreaterThan(0)
                .WithMessage("The field must not be greater than 0");
        }
    }
}