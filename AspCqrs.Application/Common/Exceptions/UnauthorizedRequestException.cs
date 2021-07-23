using System;
using System.Collections.Generic;

namespace AspCqrs.Application.Common.Exceptions
{
    public class UnauthorizedRequestException : Exception
    {
        private const string ErrorMessage = "Unauthorized request.";
        
        public UnauthorizedRequestException()
            : base(ErrorMessage)
        {
            Errors = new Dictionary<string, string[]>
            {
                {"Unauthorized", new[] {new UnauthorizedAccessException().Message}}
            };
        }

        public UnauthorizedRequestException(IDictionary<string, string[]> errors)
            : base(ErrorMessage)
        {
            Errors = errors;
        }
        
        public UnauthorizedRequestException(string code, string[] description)
            : base(ErrorMessage)
        {
            Errors = new Dictionary<string, string[]> {{code, description}};
        }

        public UnauthorizedRequestException(string code, string description)
            : base(ErrorMessage)
        {
            Errors = new Dictionary<string, string[]> {{code, new[] {description}}};
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}