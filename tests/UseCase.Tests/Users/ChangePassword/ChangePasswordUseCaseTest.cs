using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCase.Tests.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();

        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_New_Password_Invalid()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldNotBeNull();
        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.INVALID_PASSWORD);
    }

    [Fact]
    public async Task Error_Current_Password_Different()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestChangePasswordBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.ShouldNotBeNull();
        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_FROM_CURRENT);
    }

    private ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
        var repository = UserUpdateOnlyRepositoryBuilder.Build(user);

        return new ChangePasswordUseCase(loggedUser, unitOfWork, passwordEncripter, repository);
    }
}
