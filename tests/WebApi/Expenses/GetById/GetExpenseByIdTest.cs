using CashFlow.Communication.Enum;
using CashFlow.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Expenses.GetById
{
    public class GetExpenseByIdTest : CashFlowClassFixture
    {
        private const string PATH = "api/Expenses";

        private readonly string _token;
        private readonly long _expenseId;
        public GetExpenseByIdTest(IntegrationTestServer testServer) : base(testServer)
        {
            _token = testServer.User_Team_Member.GetToken();
            _expenseId = testServer.Expense_MemberTeam.GetId();
        }

        [Fact]
        public async Task Success()
        {
            var response = await DoGet(requestUri: $"{PATH}/{_expenseId}", token: _token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
            responseData.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
            responseData.RootElement.GetProperty("date").GetDateTime().ShouldBeLessThanOrEqualTo(DateTime.Today);
            responseData.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);
            responseData.RootElement.GetProperty("tags").EnumerateArray().ShouldNotBeEmpty();

            var paymentType = responseData.RootElement.GetProperty("paymentType").GetInt32();
            Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue();
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Error_Expense_Not_Found(string culture)
        {
            var response = await DoGet(requestUri: $"{PATH}/30", token: _token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NOTFOUND_MSG_ERRO", new CultureInfo(culture));

            errors.ShouldHaveSingleItem();
            errors.ShouldContain(e => e.GetString() == expectedMessage);
        }
    }
}
