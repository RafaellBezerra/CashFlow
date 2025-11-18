using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using FluentValidation;
using Shouldly;

namespace Validator.Tests.Users
{
    public class PasswordValidatorTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("a")]
        [InlineData("aa")]
        [InlineData("aaa")]
        [InlineData("aaaa")]
        [InlineData("aaaaa")]
        [InlineData("aaaaaa")]
        [InlineData("aaaaaaa")]
        [InlineData("aaaaaaaa")]
        [InlineData("AAAAAAAA")]
        [InlineData("Aaaaaaaa")]
        [InlineData("A1aaaaaa")]
        public void Error_Password_Invalid(string password)
        {
            var validator = new PasswordValidator<RequestRegisterUser>();

            var result = validator.IsValid(new ValidationContext<RequestRegisterUser>(new RequestRegisterUser()), password);

            result.ShouldBeFalse();
        }
    }
}
