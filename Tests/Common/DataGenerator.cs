using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Common
{
    public static class DataGenerator
    {
        /// <summary>
        /// Clears and  then populates database with fake records
        /// </summary>
        /// <param name="context">Database context</param>
        public static DataPool RepopulateDatabase(MasterCraftBreweryContext context)
        {
            context.ClearDatabase().DetachEntities();

            DataPool dataPool = new DataPool(context);
            dataPool.ExportToDatabase();
            return dataPool;
        }

        /// <summary>
        /// Based on name of an JSON file, this method returns deserialized C# object.
        /// </summary>
        /// <typeparam name="T">Type of the object expected to be deserialized</typeparam>
        /// <param name="resourceName">Name of the JSON file with information about this object</param>
        /// <returns>Deserialized C# object</returns>
        public static T Deserialize<T>(string resourceName)
             => JsonConvert.DeserializeObject<T>(StreamUtil.GetManifestResourceString(resourceName));

        #region ExtensionMethods
        /// <summary>
        /// Deletes all rows from database in correct order
        /// </summary>
        /// <param name="context">Context of database</param>
        public static MasterCraftBreweryContext ClearDatabase(this MasterCraftBreweryContext context)
        {
            context.Events.Clear();
            context.Ingredients.Clear();
            context.Administrators.Clear();
            context.SocialMedias.Clear();
            context.Wholesales.Clear();
            context.Quotes.Clear();
            context.Companies.Clear();
            context.MenuItems.Clear();
            context.Menus.Clear();
            context.ProductServings.Clear();
            context.Servings.Clear();
            context.Products.Clear();
            context.ProductTypes.Clear();
            context.SaveChanges();
            return context;
        }

        /// <summary>
        /// Changes state of all tracked entities to be detached.
        /// </summary>
        /// <param name="context">Context on with detaching is performed</param>
        /// <returns>Context with changed/detached entities</returns>
        public static MasterCraftBreweryContext DetachEntities(this MasterCraftBreweryContext context)
        {
            List<EntityEntry> changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (EntityEntry entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
            return context;
        }

        /// <summary>
        /// Removes rows from one table in database.
        /// </summary>
        /// <typeparam name="T">Type of an entities that needs to be removed</typeparam>
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
        #endregion
    }
}
