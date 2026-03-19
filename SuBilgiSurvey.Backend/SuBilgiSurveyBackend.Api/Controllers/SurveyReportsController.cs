using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Queries;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class SurveyReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SurveyReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Rapor ekranı: tüm anketlerin özet metrikleri (filtre/arama).</summary>
    [HttpGet]
    public async Task<IActionResult> GetReportSummaries(
        [FromQuery] string? titleContains,
        [FromQuery] SurveyStatus? status,
        [FromQuery] DateTime? endDateOnOrAfter,
        [FromQuery] DateTime? startDateOnOrBefore,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetSurveyReportSummariesQuery(titleContains, status, endDateOnOrAfter, startDateOnOrBefore),
            cancellationToken);
        return Ok(result);
    }

    /// <summary>Tek anket raporu: özet istatistik, soru/şık dağılımı, dolduran/doldurmayan, cevap detayı. userSearch ile kullanıcı adında filtre.</summary>
    [HttpGet("{surveyId:int}")]
    public async Task<IActionResult> GetSurveyReport(
        int surveyId,
        [FromQuery] string? userSearch,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetSurveyReportQuery(surveyId, userSearch), cancellationToken);
        return Ok(result);
    }
}
