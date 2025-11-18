using CashFlow.Domain.Entities;

namespace WebApi.Tests.Resources
{
    public class ExpenseIdentityManager
    {
        private readonly Expense _expense;
        private readonly DateTime _expenseDate;
        public ExpenseIdentityManager(Expense expense)
        {
            _expense = expense;
            _expenseDate = expense.Date;
        }

        public long GetId() => _expense.Id;
        public DateTime GetDate() => _expenseDate;
    }
}
