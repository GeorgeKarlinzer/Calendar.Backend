using CryptLearn.Shared.Abstractions.Exceptions;

namespace CryptLearn.Modules.AccessControl.Core.Exceptions
{
    internal class ConcurrencyException : CryptLearnException
    {
        public ConcurrencyException() : base("Concurrency error occured!")
        {
        }
    }
}
