using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

public record UpdateSurveyCommand(int RouteId, UpdateSurveyDto Dto) : IRequest<SurveyDto>;

public class UpdateSurveyCommandHandler : IRequestHandler<UpdateSurveyCommand, SurveyDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateSurveyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyDto> Handle(UpdateSurveyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Surveys.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotFoundForAdmin());

        entity.Title = request.Dto.Title;
        entity.Description = request.Dto.Description;
        entity.StartDate = request.Dto.StartDate;
        entity.EndDate = request.Dto.EndDate;
        entity.Status = request.Dto.Status;

        var existingQuestions = await _context.SurveyQuestions.Where(sq => sq.SurveyId == entity.Id).ToListAsync(cancellationToken);
        _context.SurveyQuestions.RemoveRange(existingQuestions);
        for (var i = 0; i < request.Dto.QuestionIds.Count; i++)
        {
            _context.SurveyQuestions.Add(new SuBilgiSurveyBackend.Core.Entities.SurveyQuestion
            {
                SurveyId = entity.Id,
                QuestionId = request.Dto.QuestionIds[i],
                SortOrder = i
            });
        }

        var existingAssignments = await _context.SurveyAssignments.Where(sa => sa.SurveyId == entity.Id).ToListAsync(cancellationToken);
        _context.SurveyAssignments.RemoveRange(existingAssignments);
        foreach (var userId in request.Dto.AssignedUserIds)
        {
            _context.SurveyAssignments.Add(new SuBilgiSurveyBackend.Core.Entities.SurveyAssignment
            {
                SurveyId = entity.Id,
                UserId = userId
            });
        }

        await _context.SaveChangesAsync(cancellationToken);

        var withIncludes = await _context.Surveys
            .Include(s => s.SurveyQuestions.OrderBy(sq => sq.SortOrder))
            .Include(s => s.SurveyAssignments)
            .FirstAsync(s => s.Id == entity.Id, cancellationToken);
        return withIncludes.Adapt<SurveyDto>();
    }
}
