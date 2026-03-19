using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Auth.Commands;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterDto).SetValidator(new RegisterDtoValidator());
    }
}
