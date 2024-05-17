using CryptLearn.Shared.Abstractions.Cqrs;

namespace CryptLearn.Modules.AccessControl.Core.Commands;

internal record ChangePassword(string OldPassword, string NewPassword) : ICommand;
