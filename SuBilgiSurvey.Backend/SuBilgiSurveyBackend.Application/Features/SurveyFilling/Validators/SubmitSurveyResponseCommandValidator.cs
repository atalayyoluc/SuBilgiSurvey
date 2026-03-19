using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Commands;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Validators;

public class SubmitSurveyResponseCommandValidator : AbstractValidator<SubmitSurveyResponseCommand>
{
    public SubmitSurveyResponseCommandValidator()
    {
        RuleFor(x => x.Dto.SurveyId).Greater(0);
        RuleFor(x => x.Dto.Answers).NotEmpty();
        RuleForEach(x => x.Dto.Answers).ChildRules(a =>
        {
            a.RuleFor(x => x.QuestionId).Greater(0);
            a.RuleFor(x => x.AnswerTemplateOptionId).Greater(0);
        });
    }
}
