using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Tests.Expenses.GetAll
{
    public class GetAllExpenseTest : CashFlowClassFixture
    {
        private const string PATH = "api/Expenses";

        private readonly string _token;
        public GetAllExpenseTest(IntegrationTestServer testServer) : base(testServer)
        {
            _token = testServer.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var response = await DoGet(requestUri: PATH, token: _token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("expenses").EnumerateArray().ShouldNotBeEmpty();
        }
    }
}
