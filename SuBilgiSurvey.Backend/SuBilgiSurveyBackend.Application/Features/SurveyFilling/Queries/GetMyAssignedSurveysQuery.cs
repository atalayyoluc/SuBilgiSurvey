using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

public record GetMyAssignedSurveysQuery(
    int UserId,
    DateTime? EndDateOnOrAfter = null,
    DateTime? StartDateOnOrBefore = null)
    : IRequest<List<AssignedSurveyListItemDto>>, IRequiresAuthenticatedUser;

public class GetMyAssignedSurveysQueryHandler : IRequestHandler<GetMyAssignedSurveysQuery, List<AssignedSurveyListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMyAssignedSurveysQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AssignedSurveyListItemDto>> Handle(GetMyAssignedSurveysQuery request, CancellationToken cancellationToken)
    {
        var submittedSurveyIds = await _context.SurveyResponses
            .Where(sr => sr.UserId == request.UserId)
            .Select(sr => sr.SurveyId)
            .ToListAsync(cancellationToken);

        var assignedSurveyIds = await _context.SurveyAssignments
            .Where(sa => sa.UserId == request.UserId)
            .Select(sa => sa.SurveyId)
            .ToListAsync(cancellationToken);

        var query = _context.Surveys
            .AsNoTracking()
            .Where(s =>
                assignedSurveyIds.Contains(s.Id) &&
                !submittedSurveyIds.Contains(s.Id) &&
                s.Status == SurveyStatus.Active);

        // Tarih filtresi yoksa mevcut (now) pencere mantığını koruyoruz.
        if (!request.EndDateOnOrAfter.HasValue && !request.StartDateOnOrBefore.HasValue)
        {
            var now = DateTime.UtcNow;
            query = query.Where(s => s.StartDate <= now && s.EndDate >= now);
        }
        else
        {
            // Tarih aralığı çakışması:
            // StartDate <= filterTo AND EndDate >= filterFrom
            if (request.StartDateOnOrBefore.HasValue)
                query = query.Where(s => s.StartDate <= request.StartDateOnOrBefore.Value);
            if (request.EndDateOnOrAfter.HasValue)
                query = query.Where(s => s.EndDate >= request.EndDateOnOrAfter.Value);
        }

        var surveys = await query.ToListAsync(cancellationToken);

        return surveys.Adapt<List<AssignedSurveyListItemDto>>();
    }
}
