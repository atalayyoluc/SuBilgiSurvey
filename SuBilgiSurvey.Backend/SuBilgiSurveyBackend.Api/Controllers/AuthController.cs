using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.Auth.Commands;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;
using System.Security.Claims;
using SuBilgiSurveyBackend.Application.Common.Extensions;
using SuBilgiSurveyBackend.Application.Features.Auth.Queries;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _mediator.Send(new RegisterCommand(registerDto));
        return Ok(result);
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await _mediator.Send(new LoginQuery(loginDto));
        return Ok(token);
    }

    [Authorize(AuthenticationSchemes = "RefreshToken", Policy = "RefreshToken")]
    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        var userId = int.Parse(User.FindFirst(ClaimExtensions.NameIdentifier)?.Value ?? "0");

        var token = await _mediator.Send(new RefreshQuery(userId));
        return Ok(token);
    }

}