using System.Transactions;

namespace Ng.Pass.Server.Core.Transactions;

public class TransactionWrapper
{
    /// <summary>
    /// Executes the provided operation within a transaction scope and returns the result.
    /// If the operation completes successfully, the transaction is committed. Otherwise, it is rolled back.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="operation">The asynchronous operation to execute within the transaction.</param>
    /// <returns>The result of the operation.</returns>
    public static async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var result = await operation();

        transactionScope.Complete();

        return result;
    }

    /// <summary>
    /// Executes the provided operation within a transaction scope.
    /// If the operation completes successfully, the transaction is committed. Otherwise, it is rolled back.
    /// </summary>
    /// <param name="operation">The asynchronous operation to execute within the transaction.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExecuteInTransactionAsync(Func<Task> operation)
    {
        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await operation();

        transactionScope.Complete();
    }
}
