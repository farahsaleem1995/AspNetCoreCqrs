using System;
using AspCqrs.Application.Common.Interfaces;

namespace AspCqrs.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.UtcNow;
    }
}