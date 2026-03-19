using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;

/// <summary>Rapor ekranı için anket kartı / liste satırı.</summary>
public record SurveyReportSummaryListItemDto(
    int SurveyId,
    string Title,
    SurveyStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    int AssignedUserCount,
    int SubmittedCount,
    int PendingCount,
    double CompletionRatePercent);
