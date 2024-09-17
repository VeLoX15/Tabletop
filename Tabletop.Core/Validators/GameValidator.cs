using FluentValidation;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validators
{
    public class GameValidator : AbstractValidator<Game>
    {
        public GameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(30)
                .WithMessage("Name must contain only 30 characters.");

            RuleFor(x => x.GamemodeId)
                .NotEmpty()
                .WithMessage("Game Mode must be selected");

            When(x => x.GamemodeId is 1 or 4, () =>
            {
                RuleFor(x => x.NumberOfRounds)
                    .NotNull()
                    .WithMessage("Rounds must be selected");
            });

            RuleFor(x => x.Force)
                .NotEmpty()
                .WithMessage("Force must be selected");
        }
    }
}