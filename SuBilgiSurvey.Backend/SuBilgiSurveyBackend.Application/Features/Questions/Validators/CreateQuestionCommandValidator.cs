using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Questions.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Validators;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.Dto).SetValidator(new CreateQuestionValidator());
    }
}
