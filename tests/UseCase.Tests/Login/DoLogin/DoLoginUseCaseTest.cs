using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using Shouldly;

namespace UseCase.Tests.Login.DoLogin
{
    public class DoLoginUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build();
            var useCase = CreateUseCase(user, request.Password);
            request.Email = user.Email;

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(user.Name);
            result.Token.ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Error_User_Not_Found()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build();
            var useCase = CreateUseCase(user, request.Password);

            var act = async () => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<InvalidLoginException>();

            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
        }

        [Fact]
        public async Task Error_Password_Not_Match()
        {
            var user = UserBuilder.Build();
            var request = RequestLoginBuilder.Build();
            var useCase = CreateUseCase(user);
            request.Email = user.Email;

            var act = async () => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<InvalidLoginException>();

            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
        }

        private DoLoginUseCase CreateUseCase(User user, string? password = null)
        {
            var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();
            var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
            var tokenGenerator = JwtTokenGeneratorBuilder.Build();

            return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
        }
    }
}
