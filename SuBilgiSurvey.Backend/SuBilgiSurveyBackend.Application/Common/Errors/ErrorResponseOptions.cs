namespace SuBilgiSurveyBackend.Application.Common.Errors;

/// <summary>
/// ProblemDetails Instance URL tabanı (örn. Program.cs / appsettings ile set edilir).
/// </summary>
public static class ErrorResponseOptions
{
    public static string InstanceBaseUrl { get; set; } = "https://subilgisurvey.atalayyoluc.com";

    public static void SetInstanceBaseUrl(string? url)
    {
        if (!string.IsNullOrWhiteSpace(url))
            InstanceBaseUrl = url.TrimEnd('/');
    }
}
