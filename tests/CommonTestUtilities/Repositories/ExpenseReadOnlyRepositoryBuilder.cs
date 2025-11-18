using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories
{
    public class ExpenseReadOnlyRepositoryBuilder
    {
        private readonly Mock<IExpenseReadOnlyRepository> _repository;
        public ExpenseReadOnlyRepositoryBuilder()
        {
            _repository = new Mock<IExpenseReadOnlyRepository>();
        }

        public ExpenseReadOnlyRepositoryBuilder GetAll(User user, List<Expense> expenses)
        {
            _repository.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

            return this;
        }

        public ExpenseReadOnlyRepositoryBuilder GetById(User user, Expense? expense)
        {
            if (expense is not null)
                _repository.Setup(repository => repository.GetById(user, expense.Id)).ReturnsAsync(expense);

            return this;
        }

        public ExpenseReadOnlyRepositoryBuilder UserExpenseExist(User user, Expense expense)
        {

            _repository.Setup(repository => repository.UserExpenseExist(user, expense.Id)).ReturnsAsync(true);

            return this;
        }

        public ExpenseReadOnlyRepositoryBuilder FilterByMonth(User user, List<Expense> expenses)
        {
            _repository.Setup(repository => repository.FilterByMonth(user, It.IsAny<DateOnly>())).ReturnsAsync(expenses);

            return this;
        }

        public IExpenseReadOnlyRepository Build() => _repository.Object;
    }
}
