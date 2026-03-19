using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyReporting.Dtos;
using SuBilgiSurveyBackend.Core.Entities;
using SurveyResponse = SuBilgiSurveyBackend.Core.Entities.SurveyResponse;

namespace SuBilgiSurveyBackend.Application.Features.SurveyReporting.Queries;

public record GetSurveyReportQuery(int SurveyId, string? UserSearch = null) : IRequest<SurveyReportDto>;

public class GetSurveyReportQueryHandler : IRequestHandler<GetSurveyReportQuery, SurveyReportDto>
{
    private readonly IApplicationDbContext _context;

    public GetSurveyReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyReportDto> Handle(GetSurveyReportQuery request, CancellationToken cancellationToken)
    {
        var survey = await _context.Surveys.AsNoTracking()
            .Include(s => s.SurveyQuestions)
            .ThenInclude(sq => sq.Question)
            .ThenInclude(q => q.AnswerTemplate!)
            .ThenInclude(at => at.Options)
            .FirstOrDefaultAsync(s => s.Id == request.SurveyId, cancellationToken);
        if (survey == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyReportNotFound());

        var orderedQuestions = survey.SurveyQuestions.OrderBy(sq => sq.SortOrder).ToList();
        var questionSort = orderedQuestions.ToDictionary(sq => sq.QuestionId, sq => sq.SortOrder);

        var assignedUsers = await _context.SurveyAssignments
            .AsNoTracking()
            .Where(sa => sa.SurveyId == request.SurveyId)
            .Include(sa => sa.User)
            .Select(sa => new { sa.UserId, sa.User!.FullName })
            .ToListAsync(cancellationToken);

        var responses = await _context.SurveyResponses
            .AsNoTracking()
            .Where(sr => sr.SurveyId == request.SurveyId)
            .Include(sr => sr.User)
            .Include(sr => sr.Details)
            .ThenInclude(d => d.Question)
            .Include(sr => sr.Details)
            .ThenInclude(d => d.AnswerTemplateOption)
            .ToListAsync(cancellationToken);

        var search = request.UserSearch?.Trim();
        bool NameMatches(string fullName) =>
            string.IsNullOrEmpty(search) ||
            fullName.Contains(search, StringComparison.OrdinalIgnoreCase);

        var filledUserIds = responses.Select(r => r.UserId).ToHashSet();
        var usersWhoFilled = responses
            .Where(r => NameMatches(r.User!.FullName))
            .Select(r => new UserSurveyStatusDto(r.UserId, r.User.FullName, r.SubmittedAt))
            .OrderBy(u => u.FullName)
            .ToList();

        var usersWhoDidNotFill = assignedUsers
            .Where(a => !filledUserIds.Contains(a.UserId) && NameMatches(a.FullName))
            .Select(a => new UserSurveyStatusDto(a.UserId, a.FullName, null))
            .OrderBy(u => u.FullName)
            .ToList();

        var userAnswerDetails = responses
            .Where(r => NameMatches(r.User!.FullName))
            .Select(r => new UserAnswerDetailDto(
                r.UserId,
                r.User.FullName,
                r.SubmittedAt,
                r.Details
                    .OrderBy(d => questionSort.GetValueOrDefault(d.QuestionId, int.MaxValue))
                    .Select(d => new QuestionAnswerDetailDto(
                        d.QuestionId,
                        d.Question!.Text,
                        questionSort.GetValueOrDefault(d.QuestionId),
                        d.AnswerTemplateOption!.OptionText))
                    .ToList()))
            .OrderBy(u => u.FullName)
            .ToList();

        var totalAssigned = assignedUsers.Count;
        var totalSubmitted = responses.Count;
        var totalPending = Math.Max(0, totalAssigned - totalSubmitted);
        var completionRate = totalAssigned == 0
            ? 0
            : Math.Round(100.0 * totalSubmitted / totalAssigned, 1);

        var questionStatistics = BuildQuestionStatistics(orderedQuestions, responses);

        return new SurveyReportDto(
            survey.Id,
            survey.Title,
            survey.Description,
            survey.StartDate,
            survey.EndDate,
            survey.Status,
            new SurveyReportSummaryCounts(totalAssigned, totalSubmitted, totalPending, completionRate),
            questionStatistics,
            usersWhoFilled,
            usersWhoDidNotFill,
            userAnswerDetails);
    }

    private static List<QuestionStatisticsDto> BuildQuestionStatistics(
        IReadOnlyList<SurveyQuestion> orderedSurveyQuestions,
        List<SurveyResponse> responses)
    {
        var detailRows = responses.SelectMany(r => r.Details).ToList();
        var list = new List<QuestionStatisticsDto>();

        foreach (var sq in orderedSurveyQuestions)
        {
            var q = sq.Question;
            var options = q.AnswerTemplate?.Options.OrderBy(o => o.SortOrder).ToList() ?? [];
            var optionCounts = options.Select(opt => new OptionResponseCountDto(
                opt.Id,
                opt.OptionText,
                detailRows.Count(d => d.QuestionId == q.Id && d.AnswerTemplateOptionId == opt.Id))).ToList();

            list.Add(new QuestionStatisticsDto(q.Id, q.Text, sq.SortOrder, optionCounts));
        }

        return list;
    }
}
