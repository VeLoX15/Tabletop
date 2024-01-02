using FluentValidation;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validators
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        public PlayerValidator()
        {
            RuleFor(x => x.UsedForce)
                .LessThanOrEqualTo(x => x.AllowedForce)
                .WithMessage("Force points over limit");
        }
    }
}