using MediatR;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Common.Models.Identity;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Queries;

public record RefreshQuery(int UserId) : IRequest<AccessToken>, IRequiresAuthenticatedUser;
public class RefreshQueryHandler : IRequestHandler<RefreshQuery, AccessToken>
{
    private readonly IIdentityService _identityService;

    public RefreshQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AccessToken> Handle(RefreshQuery queries, CancellationToken cancellationToken)
    {
        var result = await _identityService.RefreshUserAsync(queries);

        return result;
    }
}
