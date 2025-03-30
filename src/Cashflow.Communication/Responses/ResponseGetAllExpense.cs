namespace CashFlow.Communication.Responses;
public class ResponseGetAllExpense
{
    public List<ResponseShortGetAllExpense> Expenses { get; set; } = [];
}
