using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Questions.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Validators;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.RouteId).Equal(x => x.Dto.Id)
            .WithMessage("URL'deki id ile gövdedeki id eşleşmelidir.");
        RuleFor(x => x.Dto).SetValidator(new UpdateQuestionValidator());
    }
}
