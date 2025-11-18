using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserUpdateOnlyRepository _repository;
    public ChangePasswordUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter, IUserUpdateOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _repository = repository;
    }

    public async Task Execute(RequestChangePassword request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePassword request, User loggedUser)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.Password, loggedUser.Password);

        if (passwordMatch == false)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_FROM_CURRENT));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    } 
}
