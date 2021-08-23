using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCqrs.Application.Common.Exceptions
{
    public class CommonException : Exception
    {
        public CommonException(string error)
            : base(error)
        {
        }
    }
}