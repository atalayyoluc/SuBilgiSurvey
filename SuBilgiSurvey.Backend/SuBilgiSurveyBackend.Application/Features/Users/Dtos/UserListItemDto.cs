using SuBilgiSurveyBackend.Core.Enums;

namespace SuBilgiSurveyBackend.Application.Features.Users.Dtos;

public record UserListItemDto(int Id, string FullName, string Email, UserRole Role);
