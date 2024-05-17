namespace Calendar.Identity.Core.Entities;

internal class PermissionClaim
{
    public required string Type { get; set; }
    public required string Value { get; set; }
}
