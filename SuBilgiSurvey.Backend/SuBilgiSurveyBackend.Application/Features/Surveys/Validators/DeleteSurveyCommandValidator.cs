using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Validators;

public class DeleteSurveyCommandValidator : AbstractValidator<DeleteSurveyCommand>
{
    public DeleteSurveyCommandValidator()
    {
        RuleFor(x => x.Id).Greater(0);
    }
}
