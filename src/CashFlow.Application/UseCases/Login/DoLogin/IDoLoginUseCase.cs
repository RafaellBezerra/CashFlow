using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public interface IDoLoginUseCase
    {
        Task<ResponseRegisteredUser> Execute(RequestLogin request);
    }
}
