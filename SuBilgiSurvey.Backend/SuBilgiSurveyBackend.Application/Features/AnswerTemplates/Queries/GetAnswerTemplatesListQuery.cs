using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Queries;

public record GetAnswerTemplatesListQuery : IRequest<List<AnswerTemplateDto>>;

public class GetAnswerTemplatesListQueryHandler : IRequestHandler<GetAnswerTemplatesListQuery, List<AnswerTemplateDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAnswerTemplatesListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnswerTemplateDto>> Handle(GetAnswerTemplatesListQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.AnswerTemplates
            .AsNoTracking()
            .Include(a => a.Options.OrderBy(o => o.SortOrder))
            .OrderBy(a => a.Id)
            .ToListAsync(cancellationToken);

        return list.Adapt<List<AnswerTemplateDto>>();
    }
}
