using System;

namespace Tests.Common
{
    public class PasswordGeneratorException : Exception
    {
        public PasswordGeneratorException() : base("Password is not available for specified object type.") { }
    }
}
