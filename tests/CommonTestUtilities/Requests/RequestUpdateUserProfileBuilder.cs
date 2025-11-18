using Bogus;
using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserProfileBuilder
{
    public static RequestUpdateUserProfile Build()
    {
        return new Faker<RequestUpdateUserProfile>()
                .RuleFor(u => u.Name, faker => faker.Person.FirstName)
                .RuleFor(u => u.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}
