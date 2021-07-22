using System;

namespace AspCqrs.Application.Common.Exceptions
{
    public class ForbiddenRequestException : Exception
    {
        public ForbiddenRequestException() : base() { }
    }
}