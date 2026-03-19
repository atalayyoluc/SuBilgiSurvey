using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

public record CreateAnswerTemplateCommand(CreateAnswerTemplateDto Dto) : IRequest<AnswerTemplateDto>;

public class CreateAnswerTemplateCommandHandler : IRequestHandler<CreateAnswerTemplateCommand, AnswerTemplateDto>
{
    private readonly IApplicationDbContext _context;

    public CreateAnswerTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnswerTemplateDto> Handle(CreateAnswerTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = new SuBilgiSurveyBackend.Core.Entities.AnswerTemplate
        {
            Name = request.Dto.Name
        };
        _context.AnswerTemplates.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        for (var i = 0; i < request.Dto.Options.Count; i++)
        {
            var opt = request.Dto.Options[i];
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
