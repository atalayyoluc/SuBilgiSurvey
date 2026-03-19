using Mapster;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class SurveyFillingMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Survey, AssignedSurveyListItemDto>()
            .Map(d => d.SurveyId, s => s.Id)
            .Map(d => d.Title, s => s.Title)
            .Map(d => d.Description, s => s.Description)
            .Map(d => d.StartDate, s => s.StartDate)
            .Map(d => d.EndDate, s => s.EndDate);

        config.NewConfig<Survey, SurveyToFillDto>()
            .Map(d => d.SurveyId, s => s.Id)
            .Map(d => d.Title, s => s.Title)
            .Map(d => d.Description, s => s.Description)
            .Map(d => d.Questions, s => s.SurveyQuestions
                .OrderBy(sq => sq.SortOrder)
                .Select(sq => new QuestionToFillDto(
                    sq.Question!.Id,
                    sq.Question.Text,
                    sq.Question.AnswerTemplate!.Options
                        .OrderBy(o => o.SortOrder)
                        .Select(o => new OptionToFillDto(o.Id, o.OptionText, o.SortOrder))
                        .ToList()))
                .ToList());
    }
}
