namespace Calendar.Identity.Core.Entities;

internal class UserClaim
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public required string Type { get; set; }
    public required string Value { get; set; }
}
