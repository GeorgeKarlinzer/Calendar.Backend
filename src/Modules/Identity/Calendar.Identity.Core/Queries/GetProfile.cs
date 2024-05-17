using Calendar.Identity.Core.DTOs;
using Calendar.Shared.Abstractions.Cqrs;

namespace Calendar.Identity.Core.Queries;
internal record GetProfile() : IQuery<ProfileDto>;
