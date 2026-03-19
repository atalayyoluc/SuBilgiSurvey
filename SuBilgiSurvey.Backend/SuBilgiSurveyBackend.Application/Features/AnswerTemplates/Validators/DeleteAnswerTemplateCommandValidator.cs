using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Validators;

public class DeleteAnswerTemplateCommandValidator : AbstractValidator<DeleteAnswerTemplateCommand>
{
    public DeleteAnswerTemplateCommandValidator()
    {
        RuleFor(x => x.Id).Greater(0);
    }
}
