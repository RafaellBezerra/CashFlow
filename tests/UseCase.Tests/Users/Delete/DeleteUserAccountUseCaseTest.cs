using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCase.Tests.Users.Delete;
public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var usecase = CreateUseCase(loggedUser);

        var act = async () => await usecase.Execute();

        await act.ShouldNotThrowAsync();
    }

    private DeleteUserAccountUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = UserWriteOnlyRepositoryBuilder.Build();

        return new DeleteUserAccountUseCase(loggedUser, unitOfWork, repository);
    }
}
