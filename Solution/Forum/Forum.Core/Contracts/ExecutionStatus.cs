namespace Forum.Core.Contracts
{
    public class ExecutionStatus : IExecutionStatus
    {
        public static readonly IExecutionStatus Ok = new ExecutionStatus(ExecutionStatusCode.Ok);
        public static readonly IExecutionStatus Error = new ExecutionStatus(ExecutionStatusCode.Error);
        public static readonly IExecutionStatus Exception = new ExecutionStatus(ExecutionStatusCode.Exception);
        public static readonly IExecutionStatus NotFound = new ExecutionStatus(ExecutionStatusCode.NotFound);

        private readonly ExecutionStatusCode statusCode;

        private ExecutionStatus(ExecutionStatusCode statusCode)
        {
            this.statusCode = statusCode;
        }

        public ExecutionStatusCode Status 
        { 
            get { return statusCode; } 
        }

        public bool IsOk 
        { 
            get { return statusCode.Equals(ExecutionStatusCode.Ok); } 
        }
    }
}