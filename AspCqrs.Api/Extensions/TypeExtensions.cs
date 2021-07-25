using System;

namespace AspCqrs.Api.Extensions
{
    public static class TypeExtensions
    {
        public static bool TryParse(this Type type, object obj, out object convertedObj)
        {
            try
            {
                convertedObj = Convert.ChangeType(obj, type);
                return true;
            }
            catch (Exception e)
            {
                convertedObj = null;
                return false;
            }
        }
    }
}