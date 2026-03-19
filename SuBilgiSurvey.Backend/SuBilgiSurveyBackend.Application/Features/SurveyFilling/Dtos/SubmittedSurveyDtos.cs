namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

public record SubmittedSurveyListItemDto(
    int SurveyId,
    string Title,
    string Description,
    DateTime SubmittedAt);

public record SubmittedSurveyDetailDto(
    int SurveyId,
    string Title,
    string Description,
    DateTime SubmittedAt,
    List<SubmittedQuestionDto> Questions);

public record SubmittedQuestionDto(
    int QuestionId,
    string QuestionText,
    int AnswerTemplateOptionId,
    string OptionText);

