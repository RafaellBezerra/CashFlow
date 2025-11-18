using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.Exceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;
public class UpdateUserProfileUseCase : IUpdateUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    public UpdateUserProfileUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork,
        IUserReadOnlyRepository readOnlyRepository, IUserUpdateOnlyRepository updateOnlyRepository)
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _readOnlyRepository = readOnlyRepository;
        _updateOnlyRepository = updateOnlyRepository;
    }

    public async Task Execute(RequestUpdateUserProfile request)
    {
        var loggedUser = await _loggedUser.Get();
        
        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserProfile request, string currentEmail)
    {
        var validator = new UpdateUserProfileValidator();
        var result = validator.Validate(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExist)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
