using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.SurveyFilling.Dtos;
using SuBilgiSurveyBackend.Core.Entities;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.SurveyFilling.Commands;

public record SubmitSurveyResponseCommand(SubmitSurveyResponseDto Dto, int UserId)
    : IRequest<Unit>, IRequiresAuthenticatedUser;

public class SubmitSurveyResponseCommandHandler : IRequestHandler<SubmitSurveyResponseCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SubmitSurveyResponseCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SubmitSurveyResponseCommand request, CancellationToken cancellationToken)
    {
        await EnsureUserAssignedAsync(request.Dto.SurveyId, request.UserId, cancellationToken);
        await EnsureNotAlreadySubmittedAsync(request.Dto.SurveyId, request.UserId, cancellationToken);

        var (survey, surveyQuestionIds) = await GetActiveSurveyForFillingAsync(request.Dto.SurveyId, cancellationToken);
        ValidateAnswerSetMatchesSurvey(request.Dto.Answers, surveyQuestionIds);
        await ValidateAnswersAgainstTemplatesAsync(request.Dto.Answers, surveyQuestionIds, cancellationToken);

        var now = DateTime.UtcNow;
        await PersistResponseAsync(request.Dto, request.UserId, now, cancellationToken);
        return Unit.Value;
    }

    private async Task EnsureUserAssignedAsync(int surveyId, int userId, CancellationToken cancellationToken)
    {
        var isAssigned = await _context.SurveyAssignments
            .AnyAsync(sa => sa.SurveyId == surveyId && sa.UserId == userId, cancellationToken);
        if (!isAssigned)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotAssigned());
    }

    private async Task EnsureNotAlreadySubmittedAsync(int surveyId, int userId, CancellationToken cancellationToken)
    {
        var alreadySubmitted = await _context.SurveyResponses
            .AnyAsync(sr => sr.SurveyId == surveyId && sr.UserId == userId, cancellationToken);
        if (alreadySubmitted)
            ProblemDetailsThrower.Throw(AppErrors.SurveyAlreadySubmitted());
    }

    private async Task<(Survey Survey, HashSet<int> QuestionIds)> GetActiveSurveyForFillingAsync(
        int surveyId,
        CancellationToken cancellationToken)
    {
        var survey = await _context.Surveys
            .AsNoTracking()
            .Include(s => s.SurveyQuestions)
            .FirstOrDefaultAsync(s => s.Id == surveyId, cancellationToken);
        if (survey == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotFound());
        if (survey.Status != SurveyStatus.Active)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotActive());
        var now = DateTime.UtcNow;
        if (now < survey.StartDate || now > survey.EndDate)
            ProblemDetailsThrower.Throw(AppErrors.SurveyDateRangeInvalid());

        var questionIds = survey.SurveyQuestions.Select(sq => sq.QuestionId).ToHashSet();
        return (survey, questionIds);
    }

    private static void ValidateAnswerSetMatchesSurvey(
        IReadOnlyList<QuestionAnswerDto> answers,
        HashSet<int> surveyQuestionIds)
    {
        if (answers.Count != surveyQuestionIds.Count ||
            answers.Any(a => !surveyQuestionIds.Contains(a.QuestionId)))
            ProblemDetailsThrower.Throw(AppErrors.InvalidAnswers());
    }

    private async Task ValidateAnswersAgainstTemplatesAsync(
        IReadOnlyList<QuestionAnswerDto> answers,
        IReadOnlyCollection<int> questionIds,
        CancellationToken cancellationToken)
    {
        var qIdList = questionIds.ToList();
        var questions = await _context.Questions
            .AsNoTracking()
            .Where(q => qIdList.Contains(q.Id))
            .Include(q => q.AnswerTemplate!)
            .ThenInclude(at => at.Options)
            .ToDictionaryAsync(q => q.Id, cancellationToken);

        foreach (var a in answers)
        {
            if (!questions.TryGetValue(a.QuestionId, out var question))
                ProblemDetailsThrower.Throw(AppErrors.InvalidAnswers());
            var template = question.AnswerTemplate;
            if (template == null)
                ProblemDetailsThrower.Throw(AppErrors.InvalidAnswers());
            var optionIds = template.Options.Select(o => o.Id).ToHashSet();
            if (!optionIds.Contains(a.AnswerTemplateOptionId))
                ProblemDetailsThrower.Throw(AppErrors.InvalidAnswerOption());
        }
    }

    private async Task PersistResponseAsync(
        SubmitSurveyResponseDto dto,
        int userId,
        DateTime submittedAt,
        CancellationToken cancellationToken)
    {
        var response = new SurveyResponse
        {
            SurveyId = dto.SurveyId,
            UserId = userId,
            SubmittedAt = submittedAt
        };
        _context.SurveyResponses.Add(response);

        foreach (var answer in dto.Answers)
        {
            _context.SurveyResponseDetails.Add(new SurveyResponseDetail
            {
                SurveyResponse = response,
                QuestionId = answer.QuestionId,
                AnswerTemplateOptionId = answer.AnswerTemplateOptionId
            });
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (IsDuplicateSurveyResponse(ex))
        {
            ProblemDetailsThrower.Throw(AppErrors.SurveyAlreadySubmitted());
        }
    }

    private static bool IsDuplicateSurveyResponse(DbUpdateException ex)
    {
        var msg = ex.InnerException?.Message ?? ex.Message;
        return msg.Contains("SurveyId", StringComparison.OrdinalIgnoreCase) &&
               msg.Contains("UserId", StringComparison.OrdinalIgnoreCase);
    }
}
