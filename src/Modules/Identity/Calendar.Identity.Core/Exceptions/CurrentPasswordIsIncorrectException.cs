using CryptLearn.Shared.Abstractions.Exceptions;

namespace CryptLearn.Modules.AccessControl.Core.Exceptions
{
    internal class CurrentPasswordIsIncorrectException : CryptLearnException
    {
        public CurrentPasswordIsIncorrectException() : base("Current password is incorrect")
        {
        }
    }
}
