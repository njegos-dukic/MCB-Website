#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tests.Configuration;

namespace Tests.Common
{
    public static class DataValidator
    {
        public static void CheckAllProperties<T>(T actual, T expected, string[] ignoreProps = null)
        {
            ignoreProps ??= Array.Empty<string>();

            IEnumerable<PropertyInfo> actualProps = typeof(T).GetProperties()
                                                                    .Where(prop => !ignoreProps.Contains(prop.Name));
            foreach (PropertyInfo prop in actualProps)
            {
                object? actualValue = prop.GetValue(actual);
                object? expectedValue = prop.GetValue(expected);
                if (!(actualValue == null && expectedValue == null))
                {
                    if ((actualValue == null && expectedValue != null) || (actualValue != null && expectedValue == null))
                        throw new NotEqualException("One object is null");

                    bool isPrimitiveType = actualValue.GetType().IsValueType || actualValue is string;

                    if (isPrimitiveType)
                    {
                        if (!actualValue.Equals(expectedValue))
                            throw new NotEqualException(GetObjectsAsMessage(actual, expected));
                    }
                    else if (actualValue.GetType() == typeof(DateTime))
                    {
                        if (!AreSameDates((DateTime)actualValue, (DateTime)expectedValue))
                            throw new NotEqualException(GetObjectsAsMessage(actual, expected));
                    }
                    else if (actualValue.GetType().IsClass)
                    {
                        CheckAllProperties(actualValue, expectedValue);
                    }
                    else
                    {
                        throw new NotEqualException("Type " + actualValue?.GetType().Name + " is not supported!");
                    }
                }
            }
        }

        public static void CheckCollections<T>(List<T> actualCollection, List<T> expectedCollection, string[] ignoreProps = null)
        {
            ignoreProps ??= Array.Empty<string>();
            if (actualCollection == null && expectedCollection == null)
                return;

            if ((actualCollection == null && expectedCollection != null) || (actualCollection != null && expectedCollection == null))
                throw new NotEqualException($"One collection is null. Actual collection: {actualCollection is null} and expected: {expectedCollection is null}");

            if (actualCollection.Count != expectedCollection.Count)
                throw new NotEqualException("Different count of elements: " + GetObjectsAsMessage(actualCollection, expectedCollection));

            for (int i = 0; i < expectedCollection.Count; ++i)
                CheckAllProperties(actualCollection[i], expectedCollection[i], ignoreProps);
        }

        public static bool AreSameDates(DateTime actualValue, DateTime expectedValue)
        {
            DateTime actualDateTime = actualValue;
            DateTime expectedDateTime = expectedValue;
            if (actualValue.Kind == DateTimeKind.Unspecified)
                actualDateTime = DateTime.SpecifyKind(actualValue, DateTimeKind.Utc);

            if (expectedValue.Kind == DateTimeKind.Unspecified)
                expectedDateTime = DateTime.SpecifyKind(expectedValue, DateTimeKind.Utc);

            return actualDateTime.ToUniversalTime().Equals(expectedDateTime.ToUniversalTime());
        }

        public static void CheckCollectionEquality<T, G>(List<T> actualCollection, List<G> expectedCollection, Func<T, G, bool> predicate)
        {
            if (actualCollection == null && expectedCollection == null)
                return;

            if ((actualCollection == null && expectedCollection != null) || (actualCollection != null && expectedCollection == null))
                throw new NotEqualException($"One collection is null. Actual collection: {actualCollection is null} and expected: {expectedCollection is null}");

            if (expectedCollection.Count != actualCollection.Count)
                throw new NotEqualException("Collections have different number of elements");

            if (!actualCollection.All(x => expectedCollection.Any(y => predicate.Invoke(x, y))))
                throw new NotEqualException("Collections are not equal");
            if (!expectedCollection.All(x => actualCollection.Any(y => predicate.Invoke(y, x))))
                throw new NotEqualException("Collections are not equal");
        }

        public static string GetObjectsAsMessage(object actual, object expected)
            => "Should be" + Environment.NewLine
                + JsonConvert.SerializeObject(expected) + Environment.NewLine
                + " but was " + Environment.NewLine
                + JsonConvert.SerializeObject(actual);

        public static bool IsFileSaved(string expectedFilePath, string relativeFilePath)
        {
            if (relativeFilePath == null)
                return false;
            string fileStorageLocation = TestConfigurationManager.GetValue("fileStorageLocation");
            string filePath = Directory.GetFiles(fileStorageLocation, relativeFilePath).FirstOrDefault();
            if (filePath == null)
                return false;
            return File.ReadAllBytes(filePath).Length == File.ReadAllBytes(expectedFilePath).Length;
        }
    }

    public class NotEqualException : Exception
    {
        public NotEqualException(string message) : base(message) { }
    }
}
