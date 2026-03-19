using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Validators;

public class CreateSurveyCommandValidator : AbstractValidator<CreateSurveyCommand>
{
    public CreateSurveyCommandValidator()
    {
        RuleFor(x => x.Dto).SetValidator(new CreateSurveyValidator());
    }
}
