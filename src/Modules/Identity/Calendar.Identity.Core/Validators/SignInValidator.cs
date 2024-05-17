using CryptLearn.Modules.AccessControl.Core.Commands;
using CryptLearn.Modules.AccessControl.Core.DAL.Repositories;
using CryptLearn.Modules.AccessControl.Core.Entities;
using CryptLearn.Modules.AccessControl.Core.Exceptions;
using CryptLearn.Modules.AccessControl.Core.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CryptLearn.Modules.AccessControl.Core.Validators;
internal class SignInValidator : AbstractValidator<SignIn>
{
	public SignInValidator(IUsersRepository usersRepository, IPasswordHasher<User> passwordHasher)
	{
        RuleFor(x => x.Email)
            .MustAsync((email, ctk) => usersRepository.GetAll().AnyAsync(x => x.Email == email, ctk))
            .WithMessage("Wrong credentials");


        RuleFor(x => x.Password)
            .MustAsync(async (command, password, ctk) =>
            {
                var user = await usersRepository.GetAll().FirstOrDefaultAsync(x => x.Email == command.Email, ctk);
                if(user is null)
                {
                    return false;
                }
                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return result is not PasswordVerificationResult.Failed;
            })
            .WithMessage("Wrong credentials");
    }
}
