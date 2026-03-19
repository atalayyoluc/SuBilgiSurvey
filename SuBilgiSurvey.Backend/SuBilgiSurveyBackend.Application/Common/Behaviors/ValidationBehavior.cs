using FluentValidation;
using MediatR;
using SuBilgiSurveyBackend.Application.Common.Errors;

namespace SuBilgiSurveyBackend.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var detail = string.Join(" ", failures.Select(f =>
            string.IsNullOrEmpty(f.PropertyName)
                ? f.ErrorMessage
                : $"{f.PropertyName}: {f.ErrorMessage}"));

        ProblemDetailsThrower.Throw(AppErrors.ValidationFailed(detail));
        return default!; // unreachable
    }
}
