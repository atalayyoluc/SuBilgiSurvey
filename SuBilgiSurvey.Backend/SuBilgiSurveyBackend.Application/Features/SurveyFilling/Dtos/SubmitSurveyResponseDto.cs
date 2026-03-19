namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

public record SubmitSurveyResponseDto(int SurveyId, List<QuestionAnswerDto> Answers);

public record QuestionAnswerDto(int QuestionId, int AnswerTemplateOptionId);
