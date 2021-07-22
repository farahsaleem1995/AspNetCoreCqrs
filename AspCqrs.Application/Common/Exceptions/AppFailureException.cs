using System;
using System.Collections.Generic;

namespace AspCqrs.Application.Common.Exceptions
{
    public class AppFailureException : Exception
    {
        public AppFailureException(IDictionary<string, string[]> errors)
            : base("Failed to perform the specified action.")
        {
            Errors = errors;
        }
        
        public IDictionary<string, string[]> Errors { get; }
    }
}