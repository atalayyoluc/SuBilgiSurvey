using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Queries;

public record GetAnswerTemplateByIdQuery(int Id) : IRequest<AnswerTemplateDto>;

public class GetAnswerTemplateByIdQueryHandler : IRequestHandler<GetAnswerTemplateByIdQuery, AnswerTemplateDto>
{
    private readonly IApplicationDbContext _context;

    public GetAnswerTemplateByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerTemplateDto> Handle(GetAnswerTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnswerTemplates
            .AsNoTracking()
            .Include(a => a.Options.OrderBy(o => o.SortOrder))
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.AnswerTemplateNotFound());

        return entity!.Adapt<AnswerTemplateDto>();
    }
}
