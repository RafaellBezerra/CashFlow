using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestChangePasswordBuilder
{
    public static RequestChangePassword Build()
    {
        return new Faker<RequestChangePassword>()
            .RuleFor(u => u.Password, faker => faker.Internet.Password())
            .RuleFor(u => u.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
