using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Tests.Common
{
    public static class StreamUtil
    {
        public static string GetManifestResourceName(string resourceName)
          => Assembly.GetExecutingAssembly()
                     .GetManifestResourceNames()
                     .Single(str => str.EndsWith(resourceName, StringComparison.InvariantCulture));

        public static Stream GetManifestResourceStream(string resourceName)
            => Assembly.GetExecutingAssembly().GetManifestResourceStream(GetManifestResourceName(resourceName));

        public static string GetManifestResourceString(string resourceName)
        {
            using StreamReader sr = new StreamReader(GetManifestResourceStream(resourceName));
            return sr.ReadToEnd().Replace(Environment.NewLine, " ", StringComparison.InvariantCulture) ?? string.Empty;
        }
    }
}
