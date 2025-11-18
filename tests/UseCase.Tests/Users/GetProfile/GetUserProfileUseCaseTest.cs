using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using Shouldly;

namespace UseCase.Tests.Users.GetProfile;
public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUserCase(loggedUser);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(loggedUser.Name);
        result.Email.ShouldBe(loggedUser.Email);
    }

    private GetUserProfileUseCase CreateUserCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = MapperBuilder.Build();

        return new GetUserProfileUseCase(loggedUser, mapper);
    }
}
