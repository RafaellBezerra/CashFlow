namespace CashFlow.Communication.Responses;
public class ResponseError
{
    public List<string> ErrorMessage { get; set; }
    public ResponseError(List<string> errorMessages) => ErrorMessage = errorMessages;
    public ResponseError(string errorMessages) => ErrorMessage = [errorMessages];
}
