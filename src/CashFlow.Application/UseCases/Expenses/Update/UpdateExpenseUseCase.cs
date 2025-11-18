using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Expenses.Update;
public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpenseUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public UpdateExpenseUseCase(IUnitOfWork unitOfWork, IMapper mapper, IExpenseUpdateOnlyRepository repository, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long Id, RequestExpense request)
    {
        Validation(request);

        var loggedUser = await _loggedUser.Get();

        var expense = await _repository.GetById(loggedUser, Id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.NOTFOUND_MSG_ERRO);

        _mapper.Map(request, expense);
        _repository.Update(expense);

        await _unitOfWork.Commit();
    }
    private void Validation(RequestExpense request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessage = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
