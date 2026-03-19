using MediatR;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Common.Models.Identity;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Queries;

public record LoginQuery
(LoginDto LoginDto) : IRequest<AuthenticateResult>;

public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticateResult>
{
    private readonly IIdentityService _identityService;
    public LoginQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<AuthenticateResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        return await _identityService.LoginUserAsync(query.LoginDto, cancellationToken);
    }
}