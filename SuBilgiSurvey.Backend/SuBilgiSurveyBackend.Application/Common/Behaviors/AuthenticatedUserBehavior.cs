using MediatR;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;

namespace SuBilgiSurveyBackend.Application.Common.Behaviors;

public class AuthenticatedUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IRequiresAuthenticatedUser auth && auth.UserId <= 0)
            ProblemDetailsThrower.Throw(AppErrors.AuthenticationRequired());

        return next();
    }
}
