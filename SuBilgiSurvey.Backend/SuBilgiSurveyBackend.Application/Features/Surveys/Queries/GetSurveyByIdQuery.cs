using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Queries;

public record GetSurveyByIdQuery(int Id) : IRequest<SurveyDto>;

public class GetSurveyByIdQueryHandler : IRequestHandler<GetSurveyByIdQuery, SurveyDto>
{
    private readonly IApplicationDbContext _context;

    public GetSurveyByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyDto> Handle(GetSurveyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Surveys
            .AsNoTracking()
            .Include(s => s.SurveyQuestions.OrderBy(sq => sq.SortOrder))
            .Include(s => s.SurveyAssignments)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotFoundForAdmin());

        return entity!.Adapt<SurveyDto>();
    }
}
