using CashFlow.Domain.Security.Tokens;

namespace CashFlow.API.Token;

public class HttpContextTokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        var token = authorization["Bearer ".Length..].Trim();

        return token;
    }
}
