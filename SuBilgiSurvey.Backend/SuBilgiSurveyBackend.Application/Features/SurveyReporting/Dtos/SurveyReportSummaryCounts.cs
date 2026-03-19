namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;

/// <summary>Atanan / gönderen / bekleyen sayıları ve tamamlanma oranı.</summary>
public record SurveyReportSummaryCounts(
    int TotalAssignedUsers,
    int TotalSubmitted,
    int TotalPending,
    double CompletionRatePercent);
