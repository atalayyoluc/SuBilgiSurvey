using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;

public record SurveyDto(
    int Id,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    SurveyStatus Status,
    List<int> QuestionIds,
    List<int> AssignedUserIds);
