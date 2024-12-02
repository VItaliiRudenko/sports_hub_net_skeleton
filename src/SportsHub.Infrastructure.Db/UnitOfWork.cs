using Microsoft.Extensions.Logging;

namespace SportsHub.Infrastructure.Db;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(
        AppDbContext dbContext,
        ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ExecuteInTransactionAsync(Func<Task> func)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            await func();

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during database transaction processing");
            throw;
        }
    }

    public void ExecuteInTransaction(Action action)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            action();

            _dbContext.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred during database transaction processing");
            throw;
        }
    }

    public void CommitCurrent() => _dbContext.SaveChanges();

    public Task CommitCurrentAsync() => _dbContext.SaveChangesAsync();
}