using System.Collections.Generic;
using Tests.Common;

namespace Tests.Configuration
{
    public static class TestConfigurationManager
    {
        private static readonly Configuration configuration = DataGenerator.Deserialize<Configuration>("TestConfiguration.json");

        public static string GetValue(string key)
            => configuration.KeyValues[key];

        private sealed class Configuration
        {
            public Dictionary<string, string> KeyValues = new Dictionary<string, string>();
        }
    }
}
