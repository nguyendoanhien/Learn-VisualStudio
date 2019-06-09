using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShoesStore.MyExtensions
{
    public static partial class MyExtensions
    {
        public static string ToFormatMoney(this object money)
        {
            if (decimal.TryParse(money.ToString(), out var decMoney)) return $"{decMoney:n0}";
            return "0";
        }
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        public static string GetName<T>(this T item) where T : class
        {
            if (item == null)
                return string.Empty;

            return typeof(T).GetProperties()[0].Name;
        }
      
    }
}