using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Auth.Queries;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Validators;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.LoginDto).SetValidator(new LoginDtoValidator());
    }
}
