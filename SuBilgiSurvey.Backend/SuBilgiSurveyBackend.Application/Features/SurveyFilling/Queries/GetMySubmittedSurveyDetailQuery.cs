using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Queries;

public record GetMySubmittedSurveyDetailQuery(int SurveyId, int UserId)
    : IRequest<SubmittedSurveyDetailDto>, IRequiresAuthenticatedUser;

public class GetMySubmittedSurveyDetailQueryHandler : IRequestHandler<GetMySubmittedSurveyDetailQuery, SubmittedSurveyDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetMySubmittedSurveyDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubmittedSurveyDetailDto> Handle(GetMySubmittedSurveyDetailQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.SurveyResponses
            .AsNoTracking()
            .Include(sr => sr.Survey!)
                .ThenInclude(s => s.SurveyQuestions)
                    .ThenInclude(sq => sq.Question)
                        .ThenInclude(q => q!.AnswerTemplate)
                            .ThenInclude(at => at!.Options)
            .FirstOrDefaultAsync(
                sr => sr.SurveyId == request.SurveyId && sr.UserId == request.UserId,
                cancellationToken);

        if (response?.Survey == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotFound());

        var details = await _context.SurveyResponseDetails
            .AsNoTracking()
            .Where(d => d.SurveyResponseId == response.Id)
            .ToListAsync(cancellationToken);

        var answerByQuestionId = details.ToDictionary(d => d.QuestionId, d => d.AnswerTemplateOptionId);

        var questions = response.Survey.SurveyQuestions
            .OrderBy(sq => sq.SortOrder)
            .Select(sq =>
            {
                var q = sq.Question!;
                var chosenOptionId = answerByQuestionId.TryGetValue(q.Id, out var optId) ? optId : 0;
                var optionText = q.AnswerTemplate?.Options.FirstOrDefault(o => o.Id == chosenOptionId)?.OptionText ?? "";

                return new SubmittedQuestionDto(
                    q.Id,
                    q.Text,
                    chosenOptionId,
                    optionText);
            })
            .ToList();

        return new SubmittedSurveyDetailDto(
            response.SurveyId,
            response.Survey.Title,
            response.Survey.Description,
            response.SubmittedAt,
            questions);
    }
}

