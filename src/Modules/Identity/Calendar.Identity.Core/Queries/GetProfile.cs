using CryptLearn.Modules.AccessControl.Core.DTOs;
using CryptLearn.Shared.Abstractions.Cqrs;

namespace CryptLearn.Modules.AccessControl.Core.Queries;
internal record GetProfile() : IQuery<ProfileDto>;
