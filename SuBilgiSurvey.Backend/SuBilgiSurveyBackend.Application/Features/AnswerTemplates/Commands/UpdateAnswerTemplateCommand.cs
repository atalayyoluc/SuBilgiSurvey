using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

public record UpdateAnswerTemplateCommand(int RouteId, UpdateAnswerTemplateDto Dto) : IRequest<AnswerTemplateDto>;

public class UpdateAnswerTemplateCommandHandler : IRequestHandler<UpdateAnswerTemplateCommand, AnswerTemplateDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateAnswerTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerTemplateDto> Handle(UpdateAnswerTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnswerTemplates
            .Include(a => a.Options)
            .FirstOrDefaultAsync(a => a.Id == request.Dto.Id, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.AnswerTemplateNotFound());

        entity.Name = request.Dto.Name;

        var existingOptions = await _context.AnswerTemplateOptions
            .Where(o => o.AnswerTemplateId == entity.Id)
            .ToListAsync(cancellationToken);
        _context.AnswerTemplateOptions.RemoveRange(existingOptions);

        foreach (var opt in request.Dto.Options)
        {
            _context.AnswerTemplateOptions.Add(new SuBilgiSurveyBackend.Core.Entities.AnswerTemplateOption
            {
                AnswerTemplateId = entity.Id,
                SortOrder = opt.SortOrder,
                OptionText = opt.OptionText
            });
        }
        await _context.SaveChangesAsync(cancellationToken);

        var withOptions = await _context.AnswerTemplates
            .Include(a => a.Options.OrderBy(o => o.SortOrder))
            .FirstAsync(a => a.Id == entity.Id, cancellationToken);
        return withOptions.Adapt<AnswerTemplateDto>();
    }
}
