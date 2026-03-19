using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;

public record SurveyReportDto(
    int SurveyId,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    SurveyStatus Status,
    SurveyReportSummaryCounts Summary,
    IReadOnlyList<QuestionStatisticsDto> QuestionStatistics,
    List<UserSurveyStatusDto> UsersWhoFilled,
    List<UserSurveyStatusDto> UsersWhoDidNotFill,
    List<UserAnswerDetailDto> UserAnswerDetails);

public record UserSurveyStatusDto(int UserId, string FullName, DateTime? SubmittedAt);

public record UserAnswerDetailDto(int UserId, string FullName, DateTime SubmittedAt, List<QuestionAnswerDetailDto> Answers);

public record QuestionAnswerDetailDto(
    int QuestionId,
    string QuestionText,
    int QuestionSortOrder,
    string SelectedOptionText);
