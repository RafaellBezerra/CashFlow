using CashFlow.Communication.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.Users.Delete;
public class DeleteUserAccountTest : CashFlowClassFixture
{
    private const string PATH = "api/User";

    private readonly string _token;
    private readonly string _email;
    private readonly string _password;
    public DeleteUserAccountTest(IntegrationTestServer testServer) : base(testServer)
    {
        _token = testServer.User_Team_Member.GetToken();
        _email = testServer.User_Team_Member.GetEmail();
        _password = testServer.User_Team_Member.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: PATH, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var request = new RequestLogin
        {
            Email = _email,
            Password = _password
        };

        result = await DoPost(requestUri: "api/Login", request: request); 
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
