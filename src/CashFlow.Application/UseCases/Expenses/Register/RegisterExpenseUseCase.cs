using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public RegisterExpenseUseCase(IExpenseWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRegisterExpense> Execute(RequestExpense request)
    {
        Validation(request);
        
        var loggedUser = await _loggedUser.Get();

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id;

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisterExpense>(expense);
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
