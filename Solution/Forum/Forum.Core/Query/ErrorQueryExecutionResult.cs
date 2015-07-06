using System.Collections.Generic;
using Forum.Core.Contracts;

namespace Forum.Core.Query
{
    public class ErrorQueryExecutionResult<T> : QueryExecutionResult<T>
    {
        public ErrorQueryExecutionResult(List<Error> errors = null)
            : base(default(T), errors)
        {
        }

        public override IExecutionStatus Status
        {
            get { return ExecutionStatus.Error; }
        }
    }
}