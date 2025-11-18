using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Expenses.GetById;
public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GetExpenseByIdUseCase(IMapper mapper, IExpenseReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseGetById> Execute(long Id)
    {
        var loogedUser = await _loggedUser.Get();

        var result = await _repository.GetById(loogedUser, Id);

        if (result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.NOTFOUND_MSG_ERRO);
        }

        return _mapper.Map<ResponseGetById>(result);
    }
}
