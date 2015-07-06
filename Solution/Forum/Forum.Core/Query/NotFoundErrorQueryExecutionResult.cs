using System.Collections.Generic;
using Forum.Core.Contracts;

namespace Forum.Core.Query
{
    public class NotFoundErrorQueryExecutionResult<T> : QueryExecutionResult<T>
    {
        public NotFoundErrorQueryExecutionResult(List<Error> errors = null)
            : base(default(T), errors)
        {
        }

        public override IExecutionStatus Status
        {
            get
            {
                return ExecutionStatus.NotFound;
            }
        }
    }
}