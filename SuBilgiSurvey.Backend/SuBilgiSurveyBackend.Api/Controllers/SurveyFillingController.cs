using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Commands;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class SurveyFillingController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUser;

    public SurveyFillingController(IMediator mediator, ICurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    [HttpGet("my-surveys")]
    public async Task<IActionResult> GetMySurveys(
        [FromQuery] DateTime? endDateOnOrAfter,
        [FromQuery] DateTime? startDateOnOrBefore,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMyAssignedSurveysQuery(_currentUser.UserId, endDateOnOrAfter, startDateOnOrBefore),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("my-history")]
    public async Task<IActionResult> GetMyHistory(
        [FromQuery] DateTime? endDateOnOrAfter,
        [FromQuery] DateTime? startDateOnOrBefore,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMySubmittedSurveysQuery(_currentUser.UserId, endDateOnOrAfter, startDateOnOrBefore),
            cancellationToken);
        return Ok(result);
    }

    [HttpGet("my-history/{surveyId:int}")]
    public async Task<IActionResult> GetMyHistoryDetail(int surveyId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMySubmittedSurveyDetailQuery(surveyId, _currentUser.UserId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{surveyId:int}")]
    public async Task<IActionResult> GetSurveyToFill(int surveyId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSurveyToFillQuery(surveyId, _currentUser.UserId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitResponse([FromBody] SubmitSurveyResponseDto dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new SubmitSurveyResponseCommand(dto, _currentUser.UserId), cancellationToken);
        return Ok();
    }
}
