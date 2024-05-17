namespace Calendar.Identity.Core.DTOs;

internal record ProfileDto(string UserName, Guid UserId, string Email, IEnumerable<string> Claims);
