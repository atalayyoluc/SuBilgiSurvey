namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;

public record OptionResponseCountDto(int AnswerTemplateOptionId, string OptionText, int ResponseCount);

public record QuestionStatisticsDto(
    int QuestionId,
    string QuestionText,
    int SortOrder,
    IReadOnlyList<OptionResponseCountDto> OptionCounts);
