using System;

namespace AspCqrs.Application.Common.Cache
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CachedAttribute : Attribute
    {
        public CachedAttribute(string key)
        {
            Key = key;
        }
        
        public string Key { get; }
    }
}