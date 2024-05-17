using CryptLearn.Modules.AccessControl.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CryptLearn.Modules.AccessControl.Core.Services
{
    internal class PasswordValidator : IPasswordValidator
    {
        private readonly PasswordOptions _options;

        public IdentityErrorDescriber Describer { get; }

        public PasswordValidator(Action<PasswordOptions> configureOptions)
        {
            _options = new();
            configureOptions(_options);
            Describer = new();
        }

        public IdentityResult Validate(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < _options.RequiredLength)
            {
                errors.Add(Describer.PasswordTooShort(_options.RequiredLength));
            }
            if (_options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            {
                errors.Add(Describer.PasswordRequiresNonAlphanumeric());
            }
            if (_options.RequireDigit && !password.Any(IsDigit))
            {
                errors.Add(Describer.PasswordRequiresDigit());
            }
            if (_options.RequireLowercase && !password.Any(IsLower))
            {
                errors.Add(Describer.PasswordRequiresLower());
            }
            if (_options.RequireUppercase && !password.Any(IsUpper))
            {
                errors.Add(Describer.PasswordRequiresUpper());
            }
            if (_options.RequiredUniqueChars >= 1 && password.Distinct().Count() < _options.RequiredUniqueChars)
            {
                errors.Add(Describer.PasswordRequiresUniqueChars(_options.RequiredUniqueChars));
            }
            return errors.Count == 0
                    ? IdentityResult.Success
                    : IdentityResult.Failed(errors.ToArray());
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        private bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        private bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}
