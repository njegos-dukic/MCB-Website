using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using Tests.Configuration;

namespace Tests.Common
{
    public static class DbUtilities
    {

        /// <summary>
        /// Created DbContext with refers to new (populated) database, relational or non-relational
        /// </summary>
        /// <param name="empty">If database needs to be emtpy, this parameter must be set to true</param>
        /// <param name="options">Create database based on specific type</param>
        /// <returns>Created <see cref="MasterCraftBreweryContext"/></returns>
        public static MasterCraftBreweryContext CreateNewContext(bool empty = false, DatabaseType options = DatabaseType.InMemory)
        {
            DbContextOptionsBuilder<MasterCraftBreweryContext> contextOptionsBuilder = new DbContextOptionsBuilder<MasterCraftBreweryContext>();
            if (options == DatabaseType.InMemory)
                contextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            else contextOptionsBuilder.UseMySql(TestConfigurationManager.GetValue("databaseConnectionString"));

            MasterCraftBreweryContext context = new MasterCraftBreweryContext(contextOptionsBuilder.Options);
            if (!empty)
                Repopulate(context);
            return context;
        }

        /// <summary>
        /// Creates DbContext with populated database and database type specified in active test runsettings file
        /// </summary>
        /// <param name="testContext"></param>
        /// <returns></returns>
        public static (MasterCraftBreweryContext, DataPool) CreateNewContext(TestContext testContext)
        {
            DatabaseType options = TestPropertyReader.GetDatabaseType(testContext);
            DbContextOptionsBuilder<MasterCraftBreweryContext> contextOptionsBuilder = new DbContextOptionsBuilder<MasterCraftBreweryContext>();
            if (options == DatabaseType.InMemory)
                contextOptionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            else contextOptionsBuilder.UseMySql(TestConfigurationManager.GetValue("databaseConnectionString"));

            MasterCraftBreweryContext context = new MasterCraftBreweryContext(contextOptionsBuilder.Options);
            return (context, Repopulate(context));
        }
        /// <summary>
        /// Repopulates database depending of options (SQL or No-SQL)
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        public static DataPool Repopulate(MasterCraftBreweryContext context) =>
            DataGenerator.RepopulateDatabase(context);
    }
}
