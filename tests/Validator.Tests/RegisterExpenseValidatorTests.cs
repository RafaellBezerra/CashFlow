using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enum;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Tests;
public class RegisterExpenseValidatorTests
{
    [Fact]  
    public void Sucess()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseBuilder.Builder();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseBuilder.Builder();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            x => x.ShouldHaveSingleItem(),
            x => x.ShouldContain(e=>e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_MSG_ERRO)));
    }

    [Fact]
    public void Error_Date_Future()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseBuilder.Builder();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            x => x.ShouldHaveSingleItem(),
            x => x.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.DATA_MSG_ERRO)));
    }

    [Fact]
    public void Error_PaymentType_Invalid()
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseBuilder.Builder();
        request.PaymentType = (PaymentType)7; 

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            x => x.ShouldHaveSingleItem(),
            x => x.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_MSG_ERRO)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseBuilder.Builder();
        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            x => x.ShouldHaveSingleItem(),
            x => x.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MSG_ERRO)));
    }
}
