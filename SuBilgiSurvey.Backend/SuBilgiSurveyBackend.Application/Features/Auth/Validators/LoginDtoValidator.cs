using FluentValidation;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
