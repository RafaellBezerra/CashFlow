using Bogus;
using CashFlow.Communication.Enum;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterExpenseBuilder
{
    public static RequestExpense Builder()
    {
        var faker = new Faker();
        
        return new Faker<RequestExpense>()
            .RuleFor(x => x.Title, faker => faker.Commerce.ProductName())
            .RuleFor(x => x.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(x => x.Date, faker => faker.Date.Past())
            .RuleFor(x => x.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(x => x.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}
