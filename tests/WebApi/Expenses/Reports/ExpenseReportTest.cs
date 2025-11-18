using Shouldly;
using System.Net;
using System.Net.Mime;

namespace WebApi.Tests.Expenses.Reports;
public class ExpenseReportTest : CashFlowClassFixture
{
    private const string PATH = "api/Report";

    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;
    public ExpenseReportTest(IntegrationTestServer testServer) : base(testServer)
    {
        _adminToken = testServer.User_Admin.GetToken();
        _teamMemberToken = testServer.User_Team_Member.GetToken();
        _expenseDate = testServer.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var response = await DoGet(requestUri: $"{PATH}/pdf?month={_expenseDate:Y}", token: _adminToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        response.Content.Headers.ShouldNotBeNull();
        response.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var response = await DoGet(requestUri: $"{PATH}/pdf?{_expenseDate:Y}", token: _teamMemberToken);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }


    [Fact]
    public async Task Success_Excel()
    {
        var response = await DoGet(requestUri: $"{PATH}/excel?month={_expenseDate:Y}", token: _adminToken);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        response.Content.Headers.ShouldNotBeNull();
        response.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var response = await DoGet(requestUri: $"{PATH}/excel?{_expenseDate:Y}", token: _teamMemberToken);

        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
