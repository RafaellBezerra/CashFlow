namespace CashFlow.Domain.Repositories.User;
public interface IUserUpdateOnlyRepository
{
    Task<Entities.User> GetById(long Id);
    void Update(Entities.User user);
}
