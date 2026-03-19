using System.Security.Claims;

namespace SuBilgiSurveyBackend.Application.Common.Extensions;

public static class ClaimExtensions
{
    public const string NameIdentifier = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;

    public static void AddIdentifier(this ICollection<Claim> claims, int id)
    {
        claims.Add(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
    }

    public static void AddRole(this ICollection<Claim> claims, string role)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }
}
