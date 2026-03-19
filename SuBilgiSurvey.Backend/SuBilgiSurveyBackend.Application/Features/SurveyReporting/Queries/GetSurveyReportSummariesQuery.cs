using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Queries;

public record GetSurveyReportSummariesQuery(
    string? TitleContains = null,
    SurveyStatus? Status = null,
    DateTime? EndDateOnOrAfter = null,
    DateTime? StartDateOnOrBefore = null) : IRequest<List<SurveyReportSummaryListItemDto>>;

public class GetSurveyReportSummariesQueryHandler
    : IRequestHandler<GetSurveyReportSummariesQuery, List<SurveyReportSummaryListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSurveyReportSummariesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SurveyReportSummaryListItemDto>> Handle(
        GetSurveyReportSummariesQuery request,
        CancellationToken cancellationToken)
    {
        var q = _context.Surveys.AsNoTracking().AsQueryable();

        var title = request.TitleContains?.Trim();
        if (!string.IsNullOrEmpty(title))
            q = q.Where(s => s.Title.Contains(title));
        if (request.Status.HasValue)
            q = q.Where(s => s.Status == request.Status.Value);
        if (request.EndDateOnOrAfter.HasValue)
            q = q.Where(s => s.EndDate >= request.EndDateOnOrAfter.Value);
        if (request.StartDateOnOrBefore.HasValue)
            q = q.Where(s => s.StartDate <= request.StartDateOnOrBefore.Value);

        var surveys = await q.OrderByDescending(s => s.Id).ToListAsync(cancellationToken);
        if (surveys.Count == 0)
            return [];

        var ids = surveys.Select(s => s.Id).ToList();

        var assignedBySurvey = await _context.SurveyAssignments
            .AsNoTracking()
            .Where(sa => ids.Contains(sa.SurveyId))
            .GroupBy(sa => sa.SurveyId)
            .Select(g => new { SurveyId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SurveyId, x => x.Count, cancellationToken);

        var submittedBySurvey = await _context.SurveyResponses
            .AsNoTracking()
            .Where(sr => ids.Contains(sr.SurveyId))
            .GroupBy(sr => sr.SurveyId)
            .Select(g => new { SurveyId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SurveyId, x => x.Count, cancellationToken);

        return surveys.Select(s =>
        {
            var assigned = assignedBySurvey.GetValueOrDefault(s.Id);
            var submitted = submittedBySurvey.GetValueOrDefault(s.Id);
            var pending = Math.Max(0, assigned - submitted);
            var rate = assigned == 0 ? 0 : Math.Round(100.0 * submitted / assigned, 1);
            return new SurveyReportSummaryListItemDto(
                s.Id, s.Title, s.Status, s.StartDate, s.EndDate, assigned, submitted, pending, rate);
        }).ToList();
    }
}
