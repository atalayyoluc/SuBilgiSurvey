using MediatR;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;

namespace SuBilgiSurveyBackend.Application.Features.Surveys.Commands;

public record DeleteSurveyCommand(int Id) : IRequest<Unit>;

public class DeleteSurveyCommandHandler : IRequestHandler<DeleteSurveyCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteSurveyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSurveyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Surveys.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.SurveyNotFoundForAdmin());

        _context.Surveys.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
