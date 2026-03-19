using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class SurveyResponseDetail : BaseEntity
{
    public int SurveyResponseId { get; set; }
    public SurveyResponse SurveyResponse { get; set; } = null!;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public int AnswerTemplateOptionId { get; set; }
    public AnswerTemplateOption AnswerTemplateOption { get; set; } = null!;
}
