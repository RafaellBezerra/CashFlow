using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities
{
    public class ExpenseBuilder
    {
        public static List<Expense> Collection(User user, uint count = 2)
        {
            var expenses = new List<Expense>();

            if (count == 0)
                count = 1;

            var expenseId = 1;

            for (int i = 0; i < count; i++)
            {
                var expense = Build(user);
                expense.Id = expenseId++;
                expenses.Add(expense);
            }

            return expenses;
        }
        public static Expense Build(User user)
        {
            return new Faker<Expense>()
                .RuleFor(e => e.Id, _ => 1)
                .RuleFor(e => e.Title, faker => faker.Commerce.ProductName())
                .RuleFor(e => e.Description, faker => faker.Commerce.ProductDescription())
                .RuleFor(e => e.Date, faker => faker.Date.Past())
                .RuleFor(e => e.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
                .RuleFor(e => e.PaymentType, faker => faker.PickRandom<PaymentType>())
                .RuleFor(e => e.UserId, _ => user.Id)
                .RuleFor(e => e.Tags, faker => faker.Make(1, () => new CashFlow.Domain.Entities.Tag
                {
                    Id = 1,
                    TagValue = faker.PickRandom<CashFlow.Domain.Enums.Tag>(),
                    ExpenseId = 1,
                }));
        }
    }
}
