using SuBilgiSurveyBackend.Application.Common.Models.Identity;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;
using SuBilgiSurveyBackend.Application.Features.Auth.Queries;

namespace SuBilgiSurveyBackend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<AuthenticateResult> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<AuthenticateResult> RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<AccessToken> RefreshUserAsync(RefreshQuery userRefresh, CancellationToken cancellationToken = default);
}