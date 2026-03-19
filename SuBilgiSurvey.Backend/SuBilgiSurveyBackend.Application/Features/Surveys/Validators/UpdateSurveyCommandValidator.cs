using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Validators;

public class UpdateSurveyCommandValidator : AbstractValidator<UpdateSurveyCommand>
{
    public UpdateSurveyCommandValidator()
    {
        RuleFor(x => x.RouteId).Equal(x => x.Dto.Id)
            .WithMessage("URL'deki id ile gövdedeki id eşleşmelidir.");
        RuleFor(x => x.Dto).SetValidator(new UpdateSurveyValidator());
    }
}
