using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Questions.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Validators;

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.Id).Greater(0);
    }
}
