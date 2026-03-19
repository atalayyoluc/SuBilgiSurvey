using System.Linq.Expressions;
using FluentValidation;

namespace SuBilgiSurveyBackend.Application.Common.Extensions;

public static class ValidatorsExtensions
{
    public static IRuleBuilder<T, int> Greater<T>(this IRuleBuilder<T, int> ruleBuilder, int value)
    {
        return ruleBuilder.GreaterThan(value).WithMessage($"Bu alan {value}'dan büyük olmalıdır.");
    }

    public static IRuleBuilder<T, int> GreaterOrEqual<T>(this IRuleBuilder<T, int> ruleBuilder, int value)
    {
        return ruleBuilder.GreaterThanOrEqualTo(value).WithMessage($"Bu alan {value}'a eşit veya büyük olmalıdır.");
    }

    public static IRuleBuilder<T, DateTime> EndDateAfterStart<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder,
        Expression<Func<T, DateTime>> startDateSelector)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(startDateSelector)
            .WithMessage("Bitiş tarihi başlangıç tarihinden önce veya aynı olamaz.");
    }
}
