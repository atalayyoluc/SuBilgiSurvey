using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Surveys.Queries;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Validators;

public class GetSurveyByIdQueryValidator : AbstractValidator<GetSurveyByIdQuery>
{
    public GetSurveyByIdQueryValidator()
    {
        RuleFor(x => x.Id).Greater(0);
    }
}
