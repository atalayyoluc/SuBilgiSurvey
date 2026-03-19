using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Validators;

public class GetSurveyToFillQueryValidator : AbstractValidator<GetSurveyToFillQuery>
{
    public GetSurveyToFillQueryValidator()
    {
        RuleFor(x => x.SurveyId).Greater(0);
    }
}
