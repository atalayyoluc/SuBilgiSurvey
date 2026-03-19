using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Validators;

public class CreateAnswerTemplateValidator : AbstractValidator<CreateAnswerTemplateDto>
{
    public CreateAnswerTemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Options).NotEmpty().Must(o => o.Count >= 2 && o.Count <= 4)
            .WithMessage("Option count must be between 2 and 4.");
        RuleForEach(x => x.Options).ChildRules(opt =>
        {
            opt.RuleFor(x => x.OptionText).NotEmpty().MaximumLength(500);
        });
    }
}
