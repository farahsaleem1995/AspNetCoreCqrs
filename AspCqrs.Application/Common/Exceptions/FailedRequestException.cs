using System;
using System.Collections.Generic;

namespace AspCqrs.Application.Common.Exceptions
{
    public class FailedRequestException : Exception
    {
        public FailedRequestException(IDictionary<string, string[]> errors)
            : base("Failed to perform the specified action.")
        {
            Errors = errors;
        }
        
        public IDictionary<string, string[]> Errors { get; }
    }
}