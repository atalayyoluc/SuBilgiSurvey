using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Validators;

public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionDto>
{
    public UpdateQuestionValidator()
    {
        RuleFor(x => x.Id).Greater(0);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.AnswerTemplateId).Greater(0);
    }
}
