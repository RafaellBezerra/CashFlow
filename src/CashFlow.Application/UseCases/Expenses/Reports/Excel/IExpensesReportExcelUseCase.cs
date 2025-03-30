namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;
public interface IExpensesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
