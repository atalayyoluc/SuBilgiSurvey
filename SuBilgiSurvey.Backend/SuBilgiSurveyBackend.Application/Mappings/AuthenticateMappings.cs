using Mapster;
using SuBilgiSurveyBackend.Application.Common.Models.Identity;

namespace SuBilgiSurveyBackend.Application.Mappings;

public class AuthenticateMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(AuthenticateResult, AccessToken), AuthenticateResult>()
            .Map(dest => dest, src => src.Item1)
            .Map(dest => dest.AccessToken, src => src.Item2);
    }
}
