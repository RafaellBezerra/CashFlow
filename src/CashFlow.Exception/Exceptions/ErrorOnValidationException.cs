using System.Net;

namespace CashFlow.Exception.Exceptions;
public class ErrorOnValidationException : CashFlowException
{
    private readonly List<string> _Errors;

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _Errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return _Errors;
    }
}