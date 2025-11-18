using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCase.Tests.Expenses.GetAll
{
    public class GetAllExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expenses = ExpenseBuilder.Collection(loggedUser);

            var useCase = CreateUsecase(loggedUser, expenses);

            var result = await useCase.Execute();

            result.ShouldNotBeNull();
            result.Expenses.ShouldNotBeNull();
            result.Expenses.ShouldNotBeEmpty();
            result.Expenses.ShouldAllBe(
                expense => expense.Id > 0 &&
                !string.IsNullOrWhiteSpace(expense.Title) &&
                expense.Amount > 0);
        }

        private GetAllExpenseUseCase CreateUsecase(User user, List<Expense> expenses)
        {
            var repository = new ExpenseReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetAllExpenseUseCase(repository, mapper, loggedUser);
        }
    }
}
