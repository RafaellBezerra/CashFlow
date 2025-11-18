using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCase.Tests.Expenses.Register
{
    public class RegisterExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestExpenseBuilder.Build();
            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var result = await useCase.Execute(request);

            result.ShouldNotBeNull();
            result.Title.ShouldBe(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empty()
        {
            var request = RequestExpenseBuilder.Build();
            request.Title = string.Empty;

            var user = UserBuilder.Build();
            var useCase = CreateUseCase(user);

            var act = async() => await useCase.Execute(request);

            var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
            result.GetErrors().Count.ShouldBe(1);
            result.GetErrors().ShouldContain(ResourceErrorMessages.TITLE_MSG_ERRO);
        }

        private RegisterExpenseUseCase CreateUseCase(User user)
        {
            var expenseWriteOnly = ExpenseWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new RegisterExpenseUseCase(expenseWriteOnly, unitOfWork, mapper, loggedUser);
        }
    }
}
