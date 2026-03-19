using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Validators;

public class CreateAnswerTemplateCommandValidator : AbstractValidator<CreateAnswerTemplateCommand>
{
    public CreateAnswerTemplateCommandValidator()
    {
        RuleFor(x => x.Dto).SetValidator(new CreateAnswerTemplateValidator());
    }
}
