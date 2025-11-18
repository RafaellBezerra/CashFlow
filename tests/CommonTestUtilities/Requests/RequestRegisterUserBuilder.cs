using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterUserBuilder
    {
        public static RequestRegisterUser Build()
        {
            return new Faker<RequestRegisterUser>()
                .RuleFor(u => u.Name, faker => faker.Person.FirstName)
                .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name))
                .RuleFor(u => u.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
        }
    }
}
