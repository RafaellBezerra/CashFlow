using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses;
public class ExpenseValidator : AbstractValidator<RequestExpense>
{
    public ExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_MSG_ERRO);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.DATA_MSG_ERRO);
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MSG_ERRO);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_MSG_ERRO);
        RuleFor(expense => expense.Tags).ForEach(tagRule => tagRule.IsInEnum().WithMessage(ResourceErrorMessages.TAG_TYPE_INVALID));
    }
}
