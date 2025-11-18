using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.Exceptions;

namespace CashFlow.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _repository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _tokenGenerator;
        public DoLoginUseCase(IUserReadOnlyRepository repository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
        {
            _repository = repository;
            _passwordEncripter = passwordEncripter;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
        {
            var user = await _repository.GetUserByEmail(request.Email) ?? throw new InvalidLoginException();

            var passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

            if (passwordMatch == false)
                throw new InvalidLoginException();

            return new ResponseRegisteredUser
            {
                Name = user.Name,
                Token = _tokenGenerator.Generate(user)
            };
        }
    }
}
