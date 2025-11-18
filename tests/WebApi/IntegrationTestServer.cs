using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Resources;

namespace WebApi.Tests
{
    public class IntegrationTestServer : WebApplicationFactory<Program>
    {
        public ExpenseIdentityManager Expense_MemberTeam { get; private set; } = default!;
        public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;
        public UserIdentityManager User_Team_Member { get; private set; } = default!;
        public UserIdentityManager User_Admin { get; private set; } = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test").
                ConfigureServices(services =>
                {
                    var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<CashFlowDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(serviceProvider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                    var passworEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();
                    
                    StartDatabase(dbContext, passworEncripter, tokenGenerator);
                });
        }

        private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, tokenGenerator);
            AddExpenseMemberTeam(dbContext, userTeamMember);

            var userAdmin = AddUserAdmin(dbContext, passwordEncripter, tokenGenerator);
            AddExpenseAdmin(dbContext, userAdmin);

            dbContext.SaveChanges();
        }

        private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var user = UserBuilder.Build();
            user.Id = 1;

            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = tokenGenerator.Generate(user);

            User_Team_Member = new UserIdentityManager(user, password, token);

            return user;
        }
        private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            var user = UserBuilder.Build(Roles.ADMIN);
            user.Id = 2;

            var password = user.Password;

            user.Password = passwordEncripter.Encrypt(user.Password);

            dbContext.Users.Add(user);

            var token = tokenGenerator.Generate(user);

            User_Admin = new UserIdentityManager(user, password, token);

            return user;
        }

        private void AddExpenseMemberTeam(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);
            expense.Id = 1;
            
            foreach (var tag in expense.Tags)
            {
                tag.Id = 1;
            }

            dbContext.Expenses.Add(expense);

            Expense_MemberTeam = new ExpenseIdentityManager(expense);
        }
        private void AddExpenseAdmin(CashFlowDbContext dbContext, User user)
        {
            var expense = ExpenseBuilder.Build(user);
            expense.Id = 2;

            foreach (var tag in expense.Tags)
            {
                tag.Id = 2;
            }

            dbContext.Expenses.Add(expense);

            Expense_Admin = new ExpenseIdentityManager(expense);
        }
    }

}
