using System.Reflection;

namespace AspCqrs.Api.Extensions
{
    public static class ReflectionExtensions
    {
        public static void ForceSetValue(this PropertyInfo propertyInfo, object obj, object value)
        {
            var backingFieldInfo = obj.GetType().GetField($"<{propertyInfo.Name}>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (backingFieldInfo != null)
                backingFieldInfo.SetValue(obj, value);
        }
    }
}