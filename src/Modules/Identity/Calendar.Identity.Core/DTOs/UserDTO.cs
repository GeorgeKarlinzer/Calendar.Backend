namespace CryptLearn.Modules.AccessControl.Core.DTOs
{
    internal record ProfileDto(string UserName, Guid UserId, string Email, IEnumerable<string> Claims);
}
