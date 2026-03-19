namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

public record UpdateAnswerTemplateDto(int Id, string Name, List<AnswerTemplateOptionItemDto> Options);
