using CryptLearn.Shared.Abstractions.Cqrs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace CryptLearn.Modules.AccessControl.Core.Commands;
internal record RotateToken(JwtBearerOptions Options) : ICommand<string>;