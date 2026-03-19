namespace SuBilgiSurveyBackend.Application.Common.Models.Identity;

public class AccessToken
{
    public required string Token { get; init; }
    public DateTime Expires { get; init; }
}