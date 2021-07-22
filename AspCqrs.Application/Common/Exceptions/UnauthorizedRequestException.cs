using System;
using System.Collections.Generic;

namespace AspCqrs.Application.Common.Exceptions
{
    public class UnauthorizedRequestException : Exception
    {
        public UnauthorizedRequestException()
            : base("Unauthorized request")
        {
            Errors = new Dictionary<string, string[]>
            {
                {"Unauthorized", new[] {new UnauthorizedAccessException().Message}}
            };
        }

        public UnauthorizedRequestException(IDictionary<string, string[]> errors)
            : base("Unauthorized request")
        {
            Errors = errors;
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}