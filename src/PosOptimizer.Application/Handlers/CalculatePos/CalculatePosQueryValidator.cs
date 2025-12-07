using FluentValidation;
using PosOptimizer.Application.Models.Queries;
using PosOptimizer.Common.Enums;
using PosOptimizer.Common.Extensions;

namespace PosOptimizer.Application.Handlers.CalculatePos;

public class CalculatePosQueryValidator : AbstractValidator<CalculatePosQuery>
{
    public CalculatePosQueryValidator()
    {
        RuleFor(x => x.Installment)
            .GreaterThan(0)
            .WithMessage(ErrorCode.InstallmentMustBeGreaterThanZero.GetDescription());

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage(ErrorCode.CurrencyIsRequired.GetDescription());

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(ErrorCode.AmountMustBeGreaterThanZero.GetDescription());
    }
}
