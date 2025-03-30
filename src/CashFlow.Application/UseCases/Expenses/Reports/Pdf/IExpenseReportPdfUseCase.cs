namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf
{
    public interface IExpenseReportPdfUseCase
    {
        Task<byte[]> Execute(DateOnly month);
    }
}
