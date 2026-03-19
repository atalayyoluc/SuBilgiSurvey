using SuBilgiSurveyBackend.Core.Common;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Core.Entities;

public class Survey : BaseEntity, IBaseDeletableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public SurveyStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    public ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();
    public ICollection<SurveyAssignment> SurveyAssignments { get; set; } = new List<SurveyAssignment>();
    public ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
}
