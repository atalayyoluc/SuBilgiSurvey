namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

public record AnswerTemplateDto(int Id, string Name, List<AnswerTemplateOptionItemDto> Options);
