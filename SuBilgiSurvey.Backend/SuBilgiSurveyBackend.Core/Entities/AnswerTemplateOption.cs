using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class AnswerTemplateOption : BaseEntity
{
    public int AnswerTemplateId { get; set; }
    public AnswerTemplate AnswerTemplate { get; set; } = null!;
    public int SortOrder { get; set; }
    public string OptionText { get; set; } = string.Empty;
}
