using SuBilgiSurveyBackend.Core.Common;
using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Core.Entities;

public class User : BaseEntity, IBaseDeletableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}
