using System.Collections.Generic;

namespace Forum.Core.Contracts
{
    public interface IExecutionResult
    {
        bool IsSuccessful { get; }

        IExecutionStatus Status { get; }

        List<Error> Errors { get; } 
    }
}