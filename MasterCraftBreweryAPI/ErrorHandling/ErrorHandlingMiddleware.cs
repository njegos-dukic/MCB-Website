using Core.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.ErrorHandling
{
    /// <summary>
    /// Capture synchronous and asynchronous exceptions from the HTTP pipeline and generate error responses. 
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
            => (this.next) = (next);

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected

            // Specify different custom exceptions here
            if (ex is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (ex is ApiKeyAuthenticationException) code = HttpStatusCode.Unauthorized;
            else if (ex is ForbiddenAccessException) code = HttpStatusCode.Forbidden;
            else if (ex is CryptographicException) code = HttpStatusCode.Forbidden;

            string result = JsonConvert.SerializeObject(new { error = ex.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
