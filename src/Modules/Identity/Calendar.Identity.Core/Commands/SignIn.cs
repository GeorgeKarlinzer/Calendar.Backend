using Calendar.Shared.Abstractions.Cqrs;

namespace Calendar.Identity.Core.Commands;

internal record SignIn(string Email, string Password) : ICommand;
