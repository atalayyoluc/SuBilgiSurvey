using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Commands;

public record UpdateQuestionCommand(int RouteId, UpdateQuestionDto Dto) : IRequest<QuestionDto>;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Questions.FindAsync(new object[] { request.Dto.Id }, cancellationToken);
        if (entity == null)
            ProblemDetailsThrower.Throw(AppErrors.QuestionNotFound());

        entity.Text = request.Dto.Text;
        entity.AnswerTemplateId = request.Dto.AnswerTemplateId;
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Adapt<QuestionDto>();
    }
}
