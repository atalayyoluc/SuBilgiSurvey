namespace SuBilgiSurveyBackend.Application.Common.Interfaces;

/// <summary>
/// İstekte geçerli bir kullanıcı kimliği (UserId &gt; 0) beklenir; aksi halde 401 döner.
/// </summary>
public interface IRequiresAuthenticatedUser
{
    int UserId { get; }
}
