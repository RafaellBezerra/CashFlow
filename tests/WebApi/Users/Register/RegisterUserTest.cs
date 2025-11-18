using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.Register
{
    public class RegisterUserTest : CashFlowClassFixture
    {
        private const string PATH = "api/User";
        public RegisterUserTest(IntegrationTestServer testServer) : base(testServer) { }

        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserBuilder.Build();
           
            var response = await DoPost(requestUri: PATH, request: request);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name);
            responseData.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineData))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(requestUri: PATH, request: request, culture: culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldHaveSingleItem();
            errors.ShouldContain(e => e.GetString() == expectedMessage);
        }
    }
}