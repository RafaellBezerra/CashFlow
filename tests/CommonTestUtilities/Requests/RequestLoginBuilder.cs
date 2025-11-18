using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestLoginBuilder
    {
        public static RequestLogin Build()
        {
            return new Faker<RequestLogin>()
                .RuleFor(u => u.Email, faker => faker.Internet.Email())
                .RuleFor(u => u.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}
