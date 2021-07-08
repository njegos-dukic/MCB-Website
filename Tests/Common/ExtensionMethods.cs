using Core.AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tests.Common
{
    public static class CollectionExtensionMethods
    {
        public static TDTO Map<TEntity, TDTO>(this TEntity entity)
            => Mapping.Mapper.Map<TDTO>(entity);

        public static List<T> Shuffle<T>(this List<T> list)
            => list.OrderBy(x => Guid.NewGuid()).ToList();

        public static T Random<T>(this List<T> list)
            => list.Shuffle()[0];

        public static bool IsSorted<T, TKey>(this List<T> arr, Func<T, TKey> keySelector, bool desc = false)
        {
            for (int i = 0; i < arr.Count - 1; ++i)
            {
                TKey key1 = keySelector(arr[i]);
                TKey key2 = keySelector(arr[i + 1]);
                int result = Comparer<TKey>.Default.Compare(key1, key2);
                if (desc && result < 0)
                    return false;
                if (!desc && result > 0)
                    return false;
            }
            return true;
        }
    }

    public static class CheckDatabaseConnectionsExtensionMethods
    {
        public static object GetEntityFieldValue(this object entityObj, string propertyName)
              => entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).First(x => x.Name == propertyName).GetValue(entityObj, null);

        public static IEnumerable<PropertyInfo> GetManyRelatedEntityNavigatorProperties(object entityObj)
              => entityObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.CanWrite && x.GetGetMethod().IsVirtual && x.PropertyType.IsGenericType == true);

        public static bool HasAnyRelation(this object entityObj)
        {
            IEnumerable<PropertyInfo> collectionProps = GetManyRelatedEntityNavigatorProperties(entityObj);
            foreach (var item in collectionProps)
            {
                object collectionValue = GetEntityFieldValue(entityObj, item.Name);
                if (collectionValue != null && collectionValue is IEnumerable)
                {
                    var col = collectionValue as IEnumerable;
                    if (col.GetEnumerator().MoveNext())
                        return true;
                }
            }
            return false;
        }
    }
}
