using FluentValidation;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Validators;

public class CreateSurveyValidator : AbstractValidator<CreateSurveyDto>
{
    public CreateSurveyValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).MaximumLength(2000);
        RuleFor(x => x.EndDate).EndDateAfterStart(x => x.StartDate);
        RuleFor(x => x.QuestionIds).NotEmpty();
        RuleFor(x => x.AssignedUserIds).NotEmpty();
    }
}
