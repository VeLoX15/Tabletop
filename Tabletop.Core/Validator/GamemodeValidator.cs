using FluentValidation;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validator
{
    public class GamemodeValidator : AbstractValidator<Gamemode>
    {
        public GamemodeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1024)
                .WithMessage("Description must contain only 400 characters.");

            RuleFor(x => x.Mechanic)
                .MaximumLength(1024)
                .WithMessage("Description must contain only 400 characters.");
        }
    }
}