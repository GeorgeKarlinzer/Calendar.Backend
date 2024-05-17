//using Calendar.Identity.Core.Commands;
//using Calendar.Identity.Core.Interfaces;
//using FluentValidation;

//namespace Calendar.Identity.Core.Validators;

//internal class SignUpValidation : AbstractValidator<SignUp>
//{
//    public SignUpValidation(IPasswordValidator passwordValidator)
//    {
//        RuleFor(x => x.Password)
//            .Must(password =>
//            {
//                var result = passwordValidator.Validate(password);
//                return result.Succeeded;
//            })
//            .WithMessage("Weak password");
//    }
//}
