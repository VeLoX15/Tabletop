﻿using FluentValidation;
using System.Globalization;
using Tabletop.Core.Models;

namespace Tabletop.Core.Validators
{
    public class UnitValidator : AbstractValidator<Unit>
    {
        public UnitValidator()
        {
            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Name)
                .NotEmpty()
                .WithMessage("The field must be filled")
                .MaximumLength(50)
                .WithMessage("Name must contain only 50 characters.");

            RuleFor(x => x.FractionId)
                .NotEmpty()
                .WithMessage("Faction must be selected");

            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Description)
                .MaximumLength(500)
                .WithMessage("Description must contain only 500 characters.");

            RuleFor(x => x.GetLocalization(CultureInfo.CurrentCulture).Mechanic)
                .MaximumLength(500)
                .WithMessage("Description must contain only 500 characters.");

            RuleFor(x => x.Defense)
                .NotEmpty()
                .WithMessage("Defense must be filled.");

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