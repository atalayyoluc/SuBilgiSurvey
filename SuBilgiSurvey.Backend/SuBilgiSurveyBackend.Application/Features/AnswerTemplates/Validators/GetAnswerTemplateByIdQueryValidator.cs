using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Queries;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Validators;

public class GetAnswerTemplateByIdQueryValidator : AbstractValidator<GetAnswerTemplateByIdQuery>
{
    public GetAnswerTemplateByIdQueryValidator()
    {
        RuleFor(x => x.Id).Greater(0);
    }
}
