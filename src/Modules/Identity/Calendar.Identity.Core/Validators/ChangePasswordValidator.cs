using CryptLearn.Modules.AccessControl.Core.Commands;
using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptLearn.Modules.AccessControl.Core.Validators;

internal class ChangePasswordValidator : AbstractValidator<ChangePassword>
{
    public ChangePasswordValidator(IPasswordValidator passwordValidator, IPasswordHasher<User> passwordHasher, IUsersRepository usersRepository, IHttpContextAccessor contextAccessor)
    {
        RuleFor(x => x.OldPassword)
            .MustAsync(async (password, cts) =>
            {
                var userId = Guid.Parse(contextAccessor.HttpContext.User.Identity.Name);
                var user = await usersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId, cts);
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return result != PasswordVerificationResult.Failed;
            })
            .WithMessage("Incorrect current password");

        RuleFor(x => x.NewPassword)
            .Must(password =>
            {
                var result = passwordValidator.Validate(password);
                return result.Succeeded;
            })
            .WithMessage("Weak password");
    }
}
