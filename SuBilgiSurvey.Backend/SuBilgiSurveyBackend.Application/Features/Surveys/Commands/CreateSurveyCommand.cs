using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Surveys.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

public record CreateSurveyCommand(CreateSurveyDto Dto) : IRequest<SurveyDto>;

public class CreateSurveyCommandHandler : IRequestHandler<CreateSurveyCommand, SurveyDto>
{
    private readonly IApplicationDbContext _context;

    public CreateSurveyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SurveyDto> Handle(CreateSurveyCommand request, CancellationToken cancellationToken)
    {
        var entity = new SuBilgiSurveyBackend.Core.Entities.Survey
        {
            Title = request.Dto.Title,
            Description = request.Dto.Description,
            StartDate = request.Dto.StartDate,
            EndDate = request.Dto.EndDate,
            Status = request.Dto.Status
        };
        _context.Surveys.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        for (var i = 0; i < request.Dto.QuestionIds.Count; i++)
        {
            _context.SurveyQuestions.Add(new SuBilgiSurveyBackend.Core.Entities.SurveyQuestion
            {
                SurveyId = entity.Id,
                QuestionId = request.Dto.QuestionIds[i],
                SortOrder = i
            });
        }
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
