using System.Collections.Generic;

namespace Forum.Core.Contracts
{
    public abstract class ExecutionResult : IExecutionResult
    {
        private readonly List<Error> errors;

        protected ExecutionResult()
        {
        }

        protected ExecutionResult(List<Error> errors)
        {
            this.errors = errors;
        }

        public bool IsSuccessful 
        { 
            get { return Status.IsOk; } 
        }

        public abstract IExecutionStatus Status { get; }

        public List<Error> Errors
        {
            get { return errors; }
        }
    }
}