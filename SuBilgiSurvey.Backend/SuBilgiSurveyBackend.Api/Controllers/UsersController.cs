using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.Users.Queries;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUsersListQuery(), cancellationToken);
        return Ok(result);
    }
}
