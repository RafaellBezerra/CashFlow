using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Tests.Users.GetProfile;
public class GetUserProfileTest : CashFlowClassFixture
{
    private const string PATH = "api/User";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;
    public GetUserProfileTest(IntegrationTestServer testServer) : base(testServer)
    {
        _token = testServer.User_Team_Member.GetToken();
        _userName = testServer.User_Team_Member.GetName();
        _userEmail = testServer.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoGet(requestUri: PATH, token: _token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_userName);
        responseData.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail);
    }
}
