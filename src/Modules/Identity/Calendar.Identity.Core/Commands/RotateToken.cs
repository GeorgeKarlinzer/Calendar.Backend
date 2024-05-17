using Calendar.Shared.Abstractions.Cqrs;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Calendar.Identity.Core.Commands;
internal record RotateToken(JwtBearerOptions Options) : ICommand<string>;