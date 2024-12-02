namespace SportsHub.Infrastructure.Db;

public interface IUnitOfWork
{
    void ExecuteInTransaction(Action action);
    Task ExecuteInTransactionAsync(Func<Task> func);

    void CommitCurrent();
    Task CommitCurrentAsync();
}