using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Configuration
{
    public static class TestPropertyReader
    {
        public static DatabaseType GetDatabaseType(TestContext testContext)
        {
            string databaseType = (string)testContext.Properties["databaseType"];
            return (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseType);
        }
    }
}
