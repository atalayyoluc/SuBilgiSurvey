namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

public record CreateAnswerTemplateDto(string Name, List<AnswerTemplateOptionItemDto> Options);
