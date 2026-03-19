using Mapster;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class AnswerTemplateMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AnswerTemplateOption, AnswerTemplateOptionItemDto>()
            .Map(d => d.SortOrder, s => s.SortOrder)
            .Map(d => d.OptionText, s => s.OptionText);

        config.NewConfig<AnswerTemplate, AnswerTemplateDto>()
            .Map(d => d.Id, s => s.Id)
            .Map(d => d.Name, s => s.Name)
            .Map(d => d.Options, s => s.Options.OrderBy(o => o.SortOrder).Adapt<List<AnswerTemplateOptionItemDto>>());
    }
}
