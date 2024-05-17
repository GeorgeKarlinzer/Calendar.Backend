using System.Security.Claims;

namespace CryptLearn.Modules.AccessControl.Core.Entities
{
    internal class UserClaim
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
