using Microsoft.AspNetCore.Identity;

namespace Calendar.Identity.Core.Interfaces;

internal interface IPasswordValidator
{
    IdentityResult Validate(string password);
}
