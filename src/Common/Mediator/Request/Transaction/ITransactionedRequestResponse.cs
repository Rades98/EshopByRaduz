namespace Mediator.Request.Transaction;

public interface ITransactionedRequestResponse
{
    /// <summary>
    /// Bool value whether request failed, so transaction has been rolled back
    /// </summary>
    public bool Failed { get; set; }
}
