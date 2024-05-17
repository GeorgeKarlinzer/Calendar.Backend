using CryptLearn.Modules.AccessControl.Core.DTOs;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using CryptLearn.Shared.Abstractions.Cqrs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CryptLearn.Modules.AccessControl.Core.Queries;

internal class QueriesHandler : IQueryHandler<GetProfile, ProfileDto>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IHttpContextAccessor _contextAccessor;

    public QueriesHandler(IUsersRepository usersRepository, IHttpContextAccessor contextAccessor)
    {
        _usersRepository = usersRepository;
        _contextAccessor = contextAccessor;
    }

    public async Task<ProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_contextAccessor.HttpContext.User.Identity.Name);
        var user = await _usersRepository.GetAll().Include(x => x.Claims).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        return new(user.UserName, user.Id, user.Email, user.Claims.Where(x => x.Type == "permissions").Select(x => x.Value));
    }
}