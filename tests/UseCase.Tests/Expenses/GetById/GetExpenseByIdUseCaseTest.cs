using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using Shouldly;

namespace UseCase.Tests.Expenses.GetById
{
    public class GetExpenseByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var expense = ExpenseBuilder.Build(loggedUser);

            var useCase = CreateUseCase(loggedUser, expense);

            var result = await useCase.Execute(expense.Id);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(expense.Id);
            result.Title.ShouldBe(expense.Title);
            result.Description.ShouldBe(expense.Description);
            result.Date.ShouldBe(expense.Date);
            result.Amount.ShouldBe(expense.Amount);
            result.PaymentType.ShouldBe((CashFlow.Communication.Enum.PaymentType)expense.PaymentType);

            result.Tags.ShouldNotBeNull();
            result.Tags.ShouldNotBeEmpty();
            result.Tags.ShouldBe(expense.Tags.Select(tag => (CashFlow.Communication.Enum.Tag)tag.TagValue));
        }

        [Fact]
        public async Task Error_Expense_Not_Found()
        {
            var loggedUser = UserBuilder.Build();

            var useCase = CreateUseCase(loggedUser);

            var act = async () => await useCase.Execute(Id: 8);

            var result = await act.ShouldThrowAsync<NotFoundException>();

            result.GetErrors().ShouldHaveSingleItem();
            result.GetErrors().ShouldContain(ResourceErrorMessages.NOTFOUND_MSG_ERRO);
        }

        private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
        {
            var mapper = MapperBuilder.Build();
            var repository = new ExpenseReadOnlyRepositoryBuilder().GetById(user, expense).Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetExpenseByIdUseCase(mapper, repository, loggedUser);
        }
    }
}
