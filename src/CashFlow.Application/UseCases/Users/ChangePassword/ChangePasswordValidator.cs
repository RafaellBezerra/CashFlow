using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(r => r.NewPassword).SetValidator(new PasswordValidator<RequestChangePassword>());
    }
}
