using System.Security.Claims;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Common.Interfaces;

namespace SuBilgiSurveyBackend.Api.Services;

public class HttpContextCurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimExtensions.NameIdentifier)?.Value;
            return int.TryParse(id, out var userId) ? userId : 0;
        }
    }
}
