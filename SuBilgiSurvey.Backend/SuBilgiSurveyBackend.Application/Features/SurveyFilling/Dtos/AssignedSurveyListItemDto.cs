namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

public record AssignedSurveyListItemDto(
    int SurveyId,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate);
