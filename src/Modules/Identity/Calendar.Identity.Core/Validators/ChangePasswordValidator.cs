//using Calendar.Identity.Core.Commands;
//using Calendar.Identity.Core.Entities;
//using Calendar.Identity.Core.Interfaces;
//using FluentValidation;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace Calendar.Identity.Core.Validators;

//internal class ChangePasswordValidator : AbstractValidator<ChangePassword>
//{
//    public ChangePasswordValidator(IPasswordValidator passwordValidator, IPasswordHasher<User> passwordHasher, IUsersRepository usersRepository, IHttpContextAccessor contextAccessor)
//    {
//        RuleFor(x => x.OldPassword)
//            .MustAsync(async (password, cts) =>
//            {
//                var userId = Guid.Parse(contextAccessor.HttpContext.User.Identity.Name);
//                var user = await usersRepository.GetAll().FirstOrDefaultAsync(x => x.Id == userId, cts);
//                var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
//                return result != PasswordVerificationResult.Failed;
//            })
//            .WithMessage("Incorrect current password");

//        RuleFor(x => x.NewPassword)
//            .Must(password =>
//            {
//                var result = passwordValidator.Validate(password);
//                return result.Succeeded;
//            })
//            .WithMessage("Weak password");
//    }
//}
