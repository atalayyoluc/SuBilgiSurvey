using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Queries;

public record GetQuestionsListQuery : IRequest<List<QuestionDto>>;

public class GetQuestionsListQueryHandler : IRequestHandler<GetQuestionsListQuery, List<QuestionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetQuestionsListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionDto>> Handle(GetQuestionsListQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Questions.AsNoTracking().OrderBy(q => q.Id).ToListAsync(cancellationToken);
        return list.Adapt<List<QuestionDto>>();
    }
}
