using Mapster;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Errors;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Common.Models.Identity;
using SuBilgiSurveyBackend.Application.Features.Auth.Dtos;
using SuBilgiSurveyBackend.Application.Features.Auth.Queries;
using SuBilgiSurveyBackend.Core.Entities;
using SuBilgiSurveyBackend.Core.Enums;
using SuBilgiSurveyBackend.Infrastructure.Helpers;

namespace SuBilgiSurveyBackend.Infrastructure.Identity;

public class IdentityManager : IIdentityService
{
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;

    public IdentityManager(IApplicationDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<AuthenticateResult> LoginUserAsync(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email, cancellationToken);
        if (user == null)
            ProblemDetailsThrower.Throw(AppErrors.InvalidLogin());

        var userPassword = new HashedPassword
        {
            PasswordHash = Convert.FromBase64String(user.PasswordHash),
            PasswordSalt = Convert.FromBase64String(user.PasswordSalt)
        };
        if (!HashingHelper.VerifyPasswordHash(loginDto.Password, userPassword))
            ProblemDetailsThrower.Throw(AppErrors.InvalidLogin());

        return await _tokenService.GenerateRefreshTokenAsync(user);
    }

    public async Task<AccessToken> RefreshUserAsync(RefreshQuery userRefresh, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userRefresh.UserId, cancellationToken);
        if (user == null)
            ProblemDetailsThrower.Throw(AppErrors.UserNotFound());

        return await _tokenService.GenerateAccessTokenAsync(user);
    }

    public async Task<AuthenticateResult> RegisterUserAsync(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var userControl = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == registerDto.Email, cancellationToken);
        if (userControl != null)
            ProblemDetailsThrower.Throw(AppErrors.UserAlreadyExists());

        var hashedPassword = HashingHelper.CreatePasswordHash(registerDto.Password);
        var user = new User
        {
            FullName = registerDto.FullName,
            Email = registerDto.Email,
            PasswordHash = Convert.ToBase64String(hashedPassword.PasswordHash),
            PasswordSalt = Convert.ToBase64String(hashedPassword.PasswordSalt),
            Role = UserRole.User
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return await _tokenService.GenerateRefreshTokenAsync(user);
    }
}