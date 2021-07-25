using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspCqrs.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace AspCqrs.Api.Binders
{
    // https://dejanstojanovic.net/aspnet/2019/september/mixed-model-binding-in-aspnet-core-using-custom-model-binders/
    public class RouteIdBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //Get Id value from the route  
            var routeIdStringValue = bindingContext.ActionContext.RouteData.Values["id"]?.ToString();

            //Get command model payload (JSON) from the body  
            string valueFromBody;
            using (var streamReader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                valueFromBody = await streamReader.ReadToEndAsync();
            }

            //Deserialize body content to model instance  
            var modelType = bindingContext.ModelMetadata.UnderlyingOrModelType;
            var modelInstance = JsonConvert.DeserializeObject(valueFromBody, modelType);
            var modelPropertyType = modelType.GetProperty("Id")?.GetValue(modelInstance, null)?.GetType();

            //If Id is available and it is of model property type then try to set the model instance property  
            if (!string.IsNullOrWhiteSpace(routeIdStringValue) &&
                modelPropertyType.TryParse(routeIdStringValue, out var routeIdValue))
            {
                var idProperty = modelType.GetProperties()
                    .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase));
                
                if (idProperty != null)
                    idProperty.ForceSetValue(modelInstance, routeIdValue);
            }

            bindingContext.Result = ModelBindingResult.Success(modelInstance);
        }
    }
}