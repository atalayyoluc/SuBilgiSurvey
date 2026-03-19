using Mapster;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class QuestionMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Question, QuestionDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Text, s => s.Text)
            .Map(d => d.AnswerTemplateId, s => s.AnswerTemplateId);
    }
}
