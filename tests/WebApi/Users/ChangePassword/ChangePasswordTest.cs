using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.Users.ChangePassword;
public class ChangePasswordTest : CashFlowClassFixture
{
    private const string PATH = "api/User/change-password";

    private readonly string _token;
    private readonly string _password;
    private readonly string _email;
    public ChangePasswordTest(IntegrationTestServer testServer) : base(testServer)
    {
        _token = testServer.User_Team_Member.GetToken();
        _password = testServer.User_Team_Member.GetPassword();
        _email = testServer.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordBuilder.Build();
        request.Password = _password;

        var response = await DoPut(requestUri: PATH, request: request, token: _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var requestLogin = new RequestLogin
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost(requestUri: "api/Login", requestLogin);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        requestLogin.Password = request.NewPassword;

        response = await DoPost(requestUri: "api/Login", requestLogin);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Error_Password_Different_Current_Password(string culture)
    {
        var request = RequestChangePasswordBuilder.Build();

        var response = await DoPut(requestUri: PATH, request: request, token: _token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_FROM_CURRENT", new CultureInfo(culture));

        errors.ShouldHaveSingleItem();
        errors.ShouldContain(e => e.GetString() == expectedMessage);
    }
}
