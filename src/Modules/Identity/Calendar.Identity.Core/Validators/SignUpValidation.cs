using CryptLearn.Modules.AccessControl.Core.Commands;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using FluentValidation;

namespace CryptLearn.Modules.AccessControl.Core.Validators;

internal class SignUpValidation : AbstractValidator<SignUp>
{
    public SignUpValidation(IUsersRepository usersRepository, IPasswordValidator passwordValidator)
    {
        RuleFor(x => x.Password)
            .Must(password =>
            {
                var result = passwordValidator.Validate(password);
                return result.Succeeded;
            })
            .WithMessage("Weak password");
    }
}
