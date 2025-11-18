using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;

namespace Validator.Tests.Users.Update;
public class UpdateUserProfileValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserProfileValidator();
        var request = RequestUpdateUserProfileBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_Name_Empty(string name)
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserProfileBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.NAME_EMPTY);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Error_Email_Empty(string email)
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserProfileBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserProfileValidator();

        var request = RequestUpdateUserProfileBuilder.Build();
        request.Email = "aa";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldHaveSingleItem();
        result.Errors.ShouldContain(e => e.ErrorMessage == ResourceErrorMessages.EMAIL_INVALID);
    }
}
