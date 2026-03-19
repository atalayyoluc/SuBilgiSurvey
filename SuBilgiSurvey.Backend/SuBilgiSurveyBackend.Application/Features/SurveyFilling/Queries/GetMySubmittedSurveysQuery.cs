using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

public record GetMySubmittedSurveysQuery(
    int UserId,
    DateTime? EndDateOnOrAfter = null,
    DateTime? StartDateOnOrBefore = null)
    : IRequest<List<SubmittedSurveyListItemDto>>, IRequiresAuthenticatedUser;

public class GetMySubmittedSurveysQueryHandler : IRequestHandler<GetMySubmittedSurveysQuery, List<SubmittedSurveyListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMySubmittedSurveysQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubmittedSurveyListItemDto>> Handle(GetMySubmittedSurveysQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SurveyResponses
            .AsNoTracking()
            .Where(sr => sr.UserId == request.UserId)
            .AsQueryable();

        // Tarih aralığı çakışması burada "submittedAt aralığı" olarak ele alınır.
        if (request.EndDateOnOrAfter.HasValue)
            query = query.Where(sr => sr.SubmittedAt >= request.EndDateOnOrAfter.Value);
        if (request.StartDateOnOrBefore.HasValue)
            query = query.Where(sr => sr.SubmittedAt <= request.StartDateOnOrBefore.Value);

        return await query
            .OrderByDescending(sr => sr.SubmittedAt)
            .Select(sr => new SubmittedSurveyListItemDto(
                sr.SurveyId,
                sr.Survey!.Title,
                sr.Survey.Description,
                sr.SubmittedAt))
            .ToListAsync(cancellationToken);
    }
}

