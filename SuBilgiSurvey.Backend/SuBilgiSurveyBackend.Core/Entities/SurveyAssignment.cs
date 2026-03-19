using SuBilgiSurveyBackend.Core.Common;

namespace SuBilgiSurveyBackend.Core.Entities;

public class SurveyAssignment : BaseEntity
{
    public int SurveyId { get; set; }
    public Survey Survey { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
