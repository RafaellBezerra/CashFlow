using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;
public class GetAllExpenseUseCase : IGetAllExpenseUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetAllExpenseUseCase(IExpenseReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseGetAllExpense> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(loggedUser);

        return new ResponseGetAllExpense
        {
            Expenses = _mapper.Map<List<ResponseShortGetAllExpense>>(result)
        };
    }
}
