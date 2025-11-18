using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using Shouldly;

namespace UseCase.Tests.Expenses.Update;
public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseBuilder.Build();
        var loggeduser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggeduser);

        var useCase = CreateUseCase(loggeduser, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        await act.ShouldNotThrowAsync();

        expense.Title.ShouldBe(request.Title);
        expense.Description.ShouldBe(request.Description);
        expense.Date.ShouldBe(request.Date);
        expense.Amount.ShouldBe(request.Amount);
        expense.PaymentType.ShouldBe((CashFlow.Domain.Enums.PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var request = RequestExpenseBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.TITLE_MSG_ERRO);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(Id: 8, request);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().ShouldHaveSingleItem();
        result.GetErrors().ShouldContain(ResourceErrorMessages.NOTFOUND_MSG_ERRO);
    }

    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var repository = new ExpenseUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseUseCase(unitOfWork, mapper, repository, loggedUser);
    }
}
