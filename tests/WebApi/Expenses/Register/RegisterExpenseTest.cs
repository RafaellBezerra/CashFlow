using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Expenses.Register
{
    public class RegisterExpenseTest : CashFlowClassFixture
    {
        private const string PATH = "api/Expenses";

        private readonly string _token;
        public RegisterExpenseTest(IntegrationTestServer testServer) : base(testServer)
        {
            _token = testServer.User_Team_Member.GetToken();
        }

        [Fact]
        public async Task Success()
        {
            var request = RequestExpenseBuilder.Build();

            var response = await DoPost(requestUri: PATH, request: request, token: _token);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Error_Title_Empty(string culture)
        {
            var request = RequestExpenseBuilder.Build();
            request.Title = string.Empty;

            var response = await DoPost(requestUri: PATH, request: request, token: _token, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_MSG_ERRO", new CultureInfo(culture));

            errors.ShouldHaveSingleItem();
            errors.ShouldContain(e => e.GetString() == expectedMessage);
        }
    }
}
