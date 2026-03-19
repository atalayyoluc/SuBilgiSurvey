using MediatR;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Commands;

public record DeleteQuestionCommand(int Id) : IRequest<Unit>;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Questions.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.QuestionNotFound());

        _context.Questions.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
