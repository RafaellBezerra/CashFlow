using System.Net;

namespace CashFlow.Exception.Exceptions;
public class ErrorOnValidationException : CashFlowException
{
    public List<string> Errors { get; set; }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        Errors = errorMessages;
    }

    public override List<string> GetErrors()
    {
        return Errors;
    }
}
