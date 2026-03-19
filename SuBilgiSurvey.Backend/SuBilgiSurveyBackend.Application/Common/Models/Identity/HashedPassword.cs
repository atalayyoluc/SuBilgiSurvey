namespace SuBilgiSurveyBackend.Application.Common.Models.Identity;

public class HashedPassword
{
    public required byte[] PasswordHash { get; init; }
    public required byte[] PasswordSalt { get; init; }
}