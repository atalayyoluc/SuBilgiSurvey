using Mapster;
using MediatR;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Questions.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Questions.Commands;

public record CreateQuestionCommand(CreateQuestionDto Dto) : IRequest<QuestionDto>;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = new SuBilgiSurveyBackend.Core.Entities.Question
        {
            Text = request.Dto.Text,
            AnswerTemplateId = request.Dto.AnswerTemplateId
        };
        _context.Questions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Adapt<QuestionDto>();
    }
}
