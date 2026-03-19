using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.Questions.Commands;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;
using SuBilgiSurveyBackend.Application.Features.Questions.Queries;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class QuestionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetList(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionsListQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetQuestionByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateQuestionDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateQuestionCommand(dto), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateQuestionDto dto, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateQuestionCommand(id, dto), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteQuestionCommand(id), cancellationToken);
        return NoContent();
    }
}
