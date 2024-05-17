using CryptLearn.Shared.Abstractions.Cqrs;

namespace CryptLearn.Modules.AccessControl.Core.Commands;
internal record SignUp(string UserName, string Email, string Password) : ICommand;
