using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SuBilgiSurveyBackend.Application.Common.Interfaces;
using SuBilgiSurveyBackend.Application.Features.Users.Dtos;

namespace SuBilgiSurveyBackend.Application.Features.Users.Queries;

public record GetUsersListQuery : IRequest<List<UserListItemDto>>;

public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, List<UserListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserListItemDto>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted)
            .OrderBy(u => u.FullName)
            .ToListAsync(cancellationToken);
        return users.Adapt<List<UserListItemDto>>();
    }
}
