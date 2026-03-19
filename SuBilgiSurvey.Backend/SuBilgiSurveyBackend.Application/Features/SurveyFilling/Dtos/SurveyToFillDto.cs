namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

public record SurveyToFillDto(
    int SurveyId,
    string Title,
    string Description,
    List<QuestionToFillDto> Questions);

public record QuestionToFillDto(
    int QuestionId,
    string QuestionText,
    List<OptionToFillDto> Options);

public record OptionToFillDto(int AnswerTemplateOptionId, string OptionText, int SortOrder);
