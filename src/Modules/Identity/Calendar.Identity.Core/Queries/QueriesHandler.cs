using Calendar.Identity.Core.DTOs;
using Calendar.Identity.Core.Entities;
using Calendar.Identity.Core.Interfaces;
using Calendar.Shared.Abstractions.Cqrs;
using Calendar.Shared.Abstractions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Calendar.Identity.Core.Queries;

internal class QueriesHandler(IRepository<User> usersRepository, IHttpContextAccessor contextAccessor) : IQueryHandler<GetProfile, ProfileDto>
{
    public async Task<ProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(contextAccessor.HttpContext.User.Identity.Name);
        var user = await usersRepository.GetAll().Include(x => x.Claims).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        return new(user.UserName, user.Id, user.Email, user.Claims.Where(x => x.Type == "permissions").Select(x => x.Value));
    }
}