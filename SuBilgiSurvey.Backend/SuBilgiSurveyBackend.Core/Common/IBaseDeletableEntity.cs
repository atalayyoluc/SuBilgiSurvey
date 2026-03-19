namespace SuBilgiSurveyBackend.Core.Common;

public interface IBaseDeletableEntity
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}