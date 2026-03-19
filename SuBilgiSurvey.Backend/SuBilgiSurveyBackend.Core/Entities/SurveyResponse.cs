using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class SurveyResponse : BaseEntity
{
    public int SurveyId { get; set; }
    public Survey Survey { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime SubmittedAt { get; set; }

    public ICollection<SurveyResponseDetail> Details { get; set; } = new List<SurveyResponseDetail>();
}
