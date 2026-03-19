using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Queries;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Validators;

public class GetSurveyReportSummariesQueryValidator : AbstractValidator<GetSurveyReportSummariesQuery>
{
    public GetSurveyReportSummariesQueryValidator()
    {
        RuleFor(x => x.TitleContains).MaximumLength(300).When(x => x.TitleContains != null);
    }
}
