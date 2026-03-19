using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class Question : BaseEntity, IBaseDeletableEntity
{
    public string Text { get; set; } = string.Empty;
    public int AnswerTemplateId { get; set; }
    public AnswerTemplate AnswerTemplate { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}
