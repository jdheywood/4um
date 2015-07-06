namespace Forum.Core.Contracts
{
    public interface IExecutionStatus
    {
        ExecutionStatusCode Status { get; }

        bool IsOk { get; }
    }
}