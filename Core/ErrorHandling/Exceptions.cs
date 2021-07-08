using System;

namespace Core.ErrorHandling
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized.") { }
    }

    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base("Forbidden access to the resource.") { }
    }

    public class ApiKeyAuthenticationException : Exception
    {
        public ApiKeyAuthenticationException() : base("Invalid API key") { }
    }
}
