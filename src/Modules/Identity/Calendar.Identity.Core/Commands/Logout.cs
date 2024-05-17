using Calendar.Shared.Abstractions.Cqrs;

namespace Calendar.Identity.Core.Commands;

internal record Logout() : ICommand;
