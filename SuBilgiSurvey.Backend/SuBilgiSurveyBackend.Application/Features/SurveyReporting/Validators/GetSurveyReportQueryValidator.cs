using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Queries;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Validators;

public class GetSurveyReportQueryValidator : AbstractValidator<GetSurveyReportQuery>
{
    public GetSurveyReportQueryValidator()
    {
        RuleFor(x => x.SurveyId).Greater(0);
        RuleFor(x => x.UserSearch).MaximumLength(200).When(x => x.UserSearch != null);
    }
}
