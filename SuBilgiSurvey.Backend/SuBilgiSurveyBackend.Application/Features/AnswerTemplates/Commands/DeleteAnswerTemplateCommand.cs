using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;

namespace SuBilgiSurveyBackend.Application.Features.AnswerTemplates.Commands;

public record DeleteAnswerTemplateCommand(int Id) : IRequest<Unit>;

public class DeleteAnswerTemplateCommandHandler : IRequestHandler<DeleteAnswerTemplateCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnswerTemplateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAnswerTemplateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnswerTemplates.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.AnswerTemplateNotFound());

        _context.AnswerTemplates.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
