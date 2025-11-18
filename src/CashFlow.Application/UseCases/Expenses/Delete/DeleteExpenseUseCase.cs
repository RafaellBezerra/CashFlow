using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    private readonly IExpenseReadOnlyRepository _expenseReadOnlyRepository;
    public DeleteExpenseUseCase(IExpenseWriteOnlyRepository repository, IUnitOfWork unitOfWork,
        IExpenseReadOnlyRepository expenseReadOnlyRepository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _expenseReadOnlyRepository = expenseReadOnlyRepository;
    }
    public async Task Execute(long Id)
    {
        var loggedUser = await _loggedUser.Get();

        var expenseExist = await _expenseReadOnlyRepository.UserExpenseExist(loggedUser, Id);

        if (expenseExist == false)
            throw new NotFoundException(ResourceErrorMessages.NOTFOUND_MSG_ERRO);

        await _repository.Delete(Id);

        await _unitOfWork.Commit();
    }
}
