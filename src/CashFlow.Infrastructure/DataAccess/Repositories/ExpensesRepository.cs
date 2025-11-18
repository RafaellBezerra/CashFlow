using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CashFlow.Infrastructure.DataAccess.Repositories;
internal class ExpensesRepository : IExpenseReadOnlyRepository, IExpenseWriteOnlyRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        await _dbContext.AddAsync(expense);
    }

    public async Task Delete(long Id)
    {
        var result = await _dbContext.Expenses.FirstAsync(e => e.Id == Id);

        _dbContext.Expenses.Remove(result);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbContext.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpenses().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }
    
    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpenses().FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _dbContext.Expenses.AsNoTracking().Where(e => e.UserId == user.Id && e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date).ThenBy(e => e.Title).ToListAsync();
    }

    public async Task<bool> UserExpenseExist(User user, long id)
    {
        return await _dbContext.Expenses.AnyAsync(e => e.Id == id && e.UserId == user.Id);
    }

    private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpenses()
    {
        return _dbContext.Expenses.Include(expense => expense.Tags);
    }
}
