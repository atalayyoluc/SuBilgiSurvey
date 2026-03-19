using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Queries;

public record GetSurveysListQuery(
    SurveyStatus? Status = null,
    DateTime? EndDateOnOrAfter = null,
    DateTime? StartDateOnOrBefore = null) : IRequest<List<SurveyDto>>;

public class GetSurveysListQueryHandler : IRequestHandler<GetSurveysListQuery, List<SurveyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSurveysListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SurveyDto>> Handle(GetSurveysListQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Surveys
            .AsNoTracking()
            .Include(s => s.SurveyQuestions.OrderBy(sq => sq.SortOrder))
            .Include(s => s.SurveyAssignments)
            .AsQueryable();

        if (request.Status.HasValue)
            query = query.Where(s => s.Status == request.Status.Value);
        if (request.EndDateOnOrAfter.HasValue)
            query = query.Where(s => s.EndDate >= request.EndDateOnOrAfter.Value);
        if (request.StartDateOnOrBefore.HasValue)
            query = query.Where(s => s.StartDate <= request.StartDateOnOrBefore.Value);

        var list = await query.OrderBy(s => s.Id).ToListAsync(cancellationToken);
        return list.Adapt<List<SurveyDto>>();
    }
}
