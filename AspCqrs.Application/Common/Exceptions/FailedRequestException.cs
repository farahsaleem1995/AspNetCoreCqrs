using System;
using System.Collections.Generic;

namespace AspCqrs.Application.Common.Exceptions
{
    public class FailedRequestException : Exception
    {
        private const string ErrorMessage = "Failed to perform the specified action.";
        
        public FailedRequestException(IDictionary<string, string[]> errors)
            : base(ErrorMessage)
        {
            Errors = errors;
        }
        
        public FailedRequestException(string code, string[] description)
            : base(ErrorMessage)
        {
            Errors = new Dictionary<string, string[]> {{code, description}};
        }

        public FailedRequestException(string code, string description)
            : base(ErrorMessage)
        {
            Errors = new Dictionary<string, string[]> {{code, new[] {description}}};
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}