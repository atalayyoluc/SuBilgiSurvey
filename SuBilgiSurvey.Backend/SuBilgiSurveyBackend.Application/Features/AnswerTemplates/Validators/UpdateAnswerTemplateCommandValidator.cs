using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Validators;

public class UpdateAnswerTemplateCommandValidator : AbstractValidator<UpdateAnswerTemplateCommand>
{
    public UpdateAnswerTemplateCommandValidator()
    {
        RuleFor(x => x.RouteId).Equal(x => x.Dto.Id)
            .WithMessage("URL'deki id ile gövdedeki id eşleşmelidir.");
        RuleFor(x => x.Dto).SetValidator(new UpdateAnswerTemplateValidator());
    }
}
