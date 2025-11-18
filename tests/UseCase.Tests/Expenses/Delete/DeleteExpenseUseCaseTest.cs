using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCase.Tests.Expenses.Delete
{
    public class DeleteExpenseUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, expense);

            var act = async () => await useCase.Execute(expense.Id);

            await act.ShouldNotThrowAsync();
        }

        [Fact]
        public async Task Error_Expense_Does_Not_Exist()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, expense);

            var act = async () => await useCase.Execute(Id: 15);

            var result = await act.ShouldThrowAsync<NotFoundException>();

            result.GetErrors().ShouldHaveSingleItem();
            result.GetErrors().ShouldContain(ResourceErrorMessages.NOTFOUND_MSG_ERRO);
        }

        private DeleteExpenseUseCase CreateUseCase(User user, Expense expense)
        {
            var writeOnlyRepository = ExpenseWriteOnlyRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            var readOnlyRepository = new ExpenseReadOnlyRepositoryBuilder().UserExpenseExist(user, expense).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new DeleteExpenseUseCase(writeOnlyRepository, unitOfWork, readOnlyRepository, loggedUser);
        }
    }
}
