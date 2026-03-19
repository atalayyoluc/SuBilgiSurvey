using Mapster;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class SurveyMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Survey, SurveyDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Title, s => s.Title)
            .Map(d => d.Description, s => s.Description)
            .Map(d => d.StartDate, s => s.StartDate)
            .Map(d => d.EndDate, s => s.EndDate)
            .Map(d => d.Status, s => s.Status)
            .Map(d => d.QuestionIds, s => s.SurveyQuestions.OrderBy(sq => sq.SortOrder).Select(sq => sq.QuestionId).ToList())
            .Map(d => d.AssignedUserIds, s => s.SurveyAssignments.Select(sa => sa.UserId).ToList());
    }
}
