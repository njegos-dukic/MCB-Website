using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace MasterCraftBreweryAPI.Util
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "String" },
                Description = "Api key",
                Required = false
            });

            if (IsWriteOperation(context.ApiDescription.HttpMethod))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorize",
                    In = ParameterLocation.Header,
                    Schema = new OpenApiSchema { Type = "String" },
                    Description = "Authentication token",
                    Required = false
                });
            }
        }

        private bool IsWriteOperation(string operation)
            => operation == "POST"
                || operation == "PUT"
                || operation == "PATCH"
                || operation == "DELETE";
    }
}
