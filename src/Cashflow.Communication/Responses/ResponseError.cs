namespace CashFlow.Communication.Responses;
public class ResponseError
{
    public List<string> ErrorMessages { get; set; }
    public ResponseError(List<string> errorMessages) => ErrorMessages = errorMessages;
    public ResponseError(string errorMessages) => ErrorMessages = [errorMessages];
}
