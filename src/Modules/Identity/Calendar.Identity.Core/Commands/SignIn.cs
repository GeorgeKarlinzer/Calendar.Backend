using CryptLearn.Shared.Abstractions.Cqrs;

namespace CryptLearn.Modules.AccessControl.Core.Commands;

internal record SignIn(string Email, string Password) : ICommand;
