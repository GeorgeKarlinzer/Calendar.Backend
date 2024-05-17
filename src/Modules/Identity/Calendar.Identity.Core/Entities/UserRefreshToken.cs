namespace Calendar.Identity.Core.Entities;

internal class UserRefreshToken
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public DateTime ValidTo { get; set; }
    public string Value { get; set; }
    public bool IsActive { get; set; } = true;

    protected UserRefreshToken()
    {
    }

    public UserRefreshToken(DateTime validTo, string value)
    {
        ValidTo = validTo;
        Value = value;
    }
}
