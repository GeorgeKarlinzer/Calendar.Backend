using Calendar.Shared.Abstractions.Cqrs;

namespace Calendar.Identity.Core.Commands;
internal record SignUp(string UserName, string Email, string Password) : ICommand;
