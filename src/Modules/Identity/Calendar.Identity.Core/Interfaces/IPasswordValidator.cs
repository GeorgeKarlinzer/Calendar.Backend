using Microsoft.AspNetCore.Identity;

namespace CryptLearn.Modules.AccessControl.Core.Interfaces
{
    internal interface IPasswordValidator
    {
        IdentityResult Validate(string password);
    }
}
