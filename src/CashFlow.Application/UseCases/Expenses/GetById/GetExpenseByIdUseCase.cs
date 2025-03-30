using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Expenses.GetById;
public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IMapper _mapper;
    private readonly IExpenseReadOnlyRepository _repository;
    public GetExpenseByIdUseCase(IMapper mapper, IExpenseReadOnlyRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<ResponseGetById> Execute(long Id)
    {
        var result = await _repository.GetById(Id);

        if (result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.NOTFOUND_MSG_ERRO);
        }

        return _mapper.Map<ResponseGetById>(result);
    }
}
