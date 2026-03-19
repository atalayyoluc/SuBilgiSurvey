using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

public record GetSurveyToFillQuery(int SurveyId, int UserId)
    : IRequest<SurveyToFillDto>, IRequiresAuthenticatedUser;

public class GetSurveyToFillQueryHandler : IRequestHandler<GetSurveyToFillQuery, SurveyToFillDto>
{
    private readonly IApplicationDbContext _context;

    public GetSurveyToFillQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyToFillDto> Handle(GetSurveyToFillQuery request, CancellationToken cancellationToken)
    {
        var isAssigned = await _context.SurveyAssignments
            .AnyAsync(sa => sa.SurveyId == request.SurveyId && sa.UserId == request.UserId, cancellationToken);
        if (!isAssigned)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotAssigned());

        var alreadySubmitted = await _context.SurveyResponses
            .AnyAsync(sr => sr.SurveyId == request.SurveyId && sr.UserId == request.UserId, cancellationToken);
        if (alreadySubmitted)
            ProblemDetailsThrower.Throw(AppErrors.SurveyAlreadySubmitted());

        var now = DateTime.UtcNow;
        var survey = await _context.Surveys
            .AsNoTracking()
            .Include(s => s.SurveyQuestions)
            .ThenInclude(sq => sq.Question)
            .ThenInclude(q => q!.AnswerTemplate)
            .ThenInclude(at => at!.Options)
            .FirstOrDefaultAsync(s => s.Id == request.SurveyId && s.Status == SurveyStatus.Active && s.StartDate <= now && s.EndDate >= now, cancellationToken);
        if (survey == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyCannotBeFilledNow());

        return survey.Adapt<SurveyToFillDto>();
    }
}
