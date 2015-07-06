using System.Collections.Generic;
using Forum.Core.Contracts;

namespace Forum.Core.Query
{
    public class ExceptionQueryExecutionResult<T> : QueryExecutionResult<T>
    {
        public ExceptionQueryExecutionResult(List<Error> errors = null)
            : base(default(T), errors)
        {
        }

        public override IExecutionStatus Status
        {
            get { return ExecutionStatus.Exception; }
        }
    }
}