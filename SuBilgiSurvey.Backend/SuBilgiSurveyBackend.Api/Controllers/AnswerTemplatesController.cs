using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Queries;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AnswerTemplatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnswerTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAnswerTemplatesListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAnswerTemplateByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnswerTemplateDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateAnswerTemplateCommand(dto), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAnswerTemplateDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateAnswerTemplateCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAnswerTemplateCommand(id), cancellationToken);
        return NoContent();
    }
}
