using System;

namespace AspCqrs.Application.Common.Interfaces
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}