using MediatR;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Common.Models.Identity;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Auth.Commands;

public record RegisterCommand
(RegisterDto RegisterDto) : IRequest<AuthenticateResult>;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticateResult>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthenticateResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterUserAsync(command.RegisterDto);
        return result;
    }
}
