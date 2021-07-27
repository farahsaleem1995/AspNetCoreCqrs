using System.Linq;
using System.Reflection;
using AspCqrs.Api.Extensions;
using AspCqrs.Application.Common.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspCqrs.Api.OpenApi
{
    public class SwaggerKeyPropertySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || schema.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                .Where(propertyInfo => propertyInfo.GetCustomAttribute<KeyPropertyAttribute>() != null)
                .Select(propertyInfo => propertyInfo.Name.FirstToLower());

            excludedProperties
                .Where(propertyName => schema.Properties.ContainsKey(propertyName))
                .ToList()
                .ForEach(propertyName => schema.Properties.Remove(propertyName));
        }
    }
}