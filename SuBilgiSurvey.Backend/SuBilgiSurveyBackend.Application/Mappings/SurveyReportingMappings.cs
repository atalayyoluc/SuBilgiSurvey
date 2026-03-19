using Mapster;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;
using SuBilgiSurveyBackend.Core.Entities;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class SurveyReportingMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SurveyResponse, UserSurveyStatusDto>()
            .Map(d => d.UserId, s => s.UserId)
            .Map(d => d.FullName, s => s.User!.FullName)
            .Map(d => d.SubmittedAt, s => s.SubmittedAt);

    }
}
