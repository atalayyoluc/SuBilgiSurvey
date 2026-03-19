using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.Surveys.Commands;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;
using SuBilgiSurveyBackend.Application.Features.Surveys.Queries;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SurveysController : ControllerBase
{
    private readonly IMediator _mediator;

    public SurveysController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] SurveyStatus? status,
        [FromQuery] DateTime? endDateOnOrAfter,
        [FromQuery] DateTime? startDateOnOrBefore,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSurveysListQuery(status, endDateOnOrAfter, startDateOnOrBefore), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSurveyByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSurveyDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateSurveyCommand(dto), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSurveyDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateSurveyCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteSurveyCommand(id), cancellationToken);
        return NoContent();
    }
}
