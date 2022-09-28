using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

namespace UnitTestDemo.Infrustracture
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExcelDataAttribute : DataAttribute
    {
        static ExcelDataAttribute()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }
        public ExcelDataAttribute(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }

        public string SheetName { get; set; }

        /// <summary>
        /// Data start from which row(zero-based)
        /// </summary>
        public int StartRow { get; set; } = 1;


        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            var pars = testMethod.GetParameters();

            return ReadData(FileName, pars.Select(par => par.ParameterType).ToArray(), StartRow, SheetName);
        }

        public static List<T> ReadData<T>(string fileName, int start = 1, string sheetName = null)
        {
            var properties = typeof(T).GetProperties();

            var rows = ReadData(fileName, properties.Select(p => p.PropertyType).ToArray(), start, sheetName);

            var list = new List<T>();

            foreach (var rowValues in rows)
            {
                var obj = Activator.CreateInstance<T>();
                for(int i = 0; i< properties.Length; i++)
                {
                    properties[i].SetValue(obj, rowValues[i]);
                }
                list.Add(obj);
            }

            return list;
        }

        public static IEnumerable<object[]> ReadData(string fileName, Type[] parameterTypes, int start = 1, string sheetName = null)
        {
            var path = GetFullFilename(fileName);

            using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = new List<object[]>();
            int row = 0;
            do
            {
                if (!string.IsNullOrEmpty(sheetName) && reader.Name != sheetName)
                    continue;

                while (reader.Read())
                {
                    if (row >= start)
                        result.Add(ConvertParameters(reader, parameterTypes));

                    row++;
                }

                // Read the first sheet only
                break;
            } while (reader.NextResult());

            return result;
            // var result = reader.AsDataSet();
        }

        static string GetFullFilename(string filename)
        {
            string executable = new Uri(Assembly.GetExecutingAssembly().Location).LocalPath;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), "TestData", filename));
        }

        private static object[] ConvertParameters(IExcelDataReader reader, IReadOnlyList<Type> parameterTypes)
        {
            var result = new object[parameterTypes.Count];
            for (var idx = 0; idx < parameterTypes.Count; idx++)
            {
                result[idx] = GetParameterValue(reader, idx, parameterTypes[idx]);
            }

            return result;
        }

        static object GetParameterValue(IExcelDataReader reader, int i, Type type)
        {
            try
            {
                var value = reader.GetValue(i);

                if (type == typeof(int))
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (ReferenceEquals(value, null))
                        return 0;
                    return Convert.ChangeType(value, type);
                }

                if (type == typeof(string))
                    return reader.GetString(i);
                if (type == typeof(DateTime))
                    return reader.GetDateTime(i);
                if (type.IsEnum)
                    return Enum.Parse(type, reader.GetString(i));

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (value == null && type.IsValueType)
                    return Activator.CreateInstance(type);
                
                return Convert.ChangeType(value, type);
            }
            catch
            {
                if (type.IsValueType)
                    return Activator.CreateInstance(type);
                return null;
            }
        }
    }
}
