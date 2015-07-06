using System.Collections.Generic;
using Forum.Core.Contracts;
using Forum.Core.Query;

namespace Forum.Core.Extensions
{
    public static class ErrorExtensions
    {
        public static ErrorQueryExecutionResult<T> ToErrorQueryExecutionResult<T>(this string errorMessage)
        {
            return new ErrorQueryExecutionResult<T>(new List<Error> { new Error { Message = errorMessage } });
        }

        public static NotFoundErrorQueryExecutionResult<T> ToNotFoundQueryExecutionResult<T>(this string errorMessage)
        {
            return new NotFoundErrorQueryExecutionResult<T>(new List<Error> { new Error { Message = errorMessage } });
        }
    }
}
