using SuBilgiSurveyBackend.Application.Common.Models.Identity;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Common.Interfaces;

public interface ITokenService
{
    Task<AccessToken> GenerateAccessTokenAsync(User user);
    Task<AuthenticateResult> GenerateRefreshTokenAsync(User user);
}
