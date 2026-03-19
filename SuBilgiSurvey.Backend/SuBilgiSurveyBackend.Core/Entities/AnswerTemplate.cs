using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class AnswerTemplate : BaseEntity, IBaseDeletableEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    public ICollection<AnswerTemplateOption> Options { get; set; } = new List<AnswerTemplateOption>();
}
