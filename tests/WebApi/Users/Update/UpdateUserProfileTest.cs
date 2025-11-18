using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.Update;
public class UpdateUserProfileTest : CashFlowClassFixture
{
    private const string PATH = "api/User";

    private readonly string _token;
    public UpdateUserProfileTest(IntegrationTestServer testServer) : base(testServer)
    {
        _token = testServer.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileBuilder.Build();

        var response = await DoPut(requestUri: PATH, request: request, token: _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserProfileBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPut(requestUri: PATH, request: request, token: _token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(e => e.GetString() == expectedMessage);
    }
}
