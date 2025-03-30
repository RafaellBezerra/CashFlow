using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Reports;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Application.UseCases.Expenses.Update;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection service)
    {
        AddUseCase(service);
        AddAutoMapper(service);
    }
    private static void AddUseCase(IServiceCollection service)
    {
        service.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        service.AddScoped<IGetAllExpenseUseCase, GetAllExpenseUseCase>();
        service.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        service.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        service.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        service.AddScoped<IExpensesReportExcelUseCase, ExpensesReportExcelUseCase>();
        service.AddScoped<IExpenseReportPdfUseCase, ExpenseReportPdfUseCase>();
    }
    private static void AddAutoMapper(IServiceCollection service)
    {
        service.AddAutoMapper(typeof(AutoMapping));
    }
}
