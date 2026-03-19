namespace SuBilgiSurveyBackend.Application.Common.Interfaces;

public interface ICurrentUserService
{
    /// <summary>Kimlik doğrulanmış kullanıcının Id'si; yoksa 0.</summary>
    int UserId { get; }
}
