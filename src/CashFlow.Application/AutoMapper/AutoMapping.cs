using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        MapRequestToEntity();
        MapExpenseToResponse();
    }
    private void MapRequestToEntity()
    {
        CreateMap<RequestExpense, Expense>();
    }
    private void MapExpenseToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpense>();
        CreateMap<Expense, ResponseShortGetAllExpense>();
        CreateMap<Expense, ResponseGetById>();
    }
}
