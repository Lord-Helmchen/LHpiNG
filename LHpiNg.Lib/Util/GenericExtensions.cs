/// This file is licensed as https://creativecommons.org/licenses/by-sa/3.0/ 
/// InjectNonNull:
/// source: https://stackoverflow.com/questions/17385472/entity-framework-only-update-values-that-are-not-null
/// author: https://stackoverflow.com/users/1715579/p-s-w-g
/// IsSimple:
/// source: https://stackoverflow.com/questions/863881/how-do-i-tell-if-a-type-is-a-simple-type-i-e-holds-a-single-value
/// author: https://stackoverflow.com/users/2658202/stefan-steinegger
/// None:
/// source: https://stackoverflow.com/questions/9027530/linq-not-any-vs-all-dont
/// author: https://stackoverflow.com/users/71059/aakashm


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LHpiNG.Util
{
    public static class GenericExtensions
    {
        /// <summary>
        /// Updates fields with non-null fields in src
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dest">T to be updated</param>
        /// <param name="src">T updated data is taken from</param>
        /// <returns></returns>
        public static T InjectNonNull<T>(this T dest, T src)
        {
            foreach (var propertyPair in PropertyLister<T, T>.PropertyMap)
            {
                var fromValue = propertyPair.Item2.GetValue(src, null);
                if (fromValue != null && propertyPair.Item1.CanWrite)
                {
                    propertyPair.Item1.SetValue(dest, fromValue, null);
                }
            }
            return dest;
        }

        static bool IsSimple(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return source == null ||  !source.Any();
        }

        public static bool None<TSource>(this IEnumerable<TSource> source,
                                         Func<TSource, bool> predicate)
        {
            return source == null || !source.Any(predicate);
        }
    }

    //building a list the of properties up front, so we only have to use reflection once:
    internal static class PropertyLister<T1, T2>
    {
        public static readonly IEnumerable<Tuple<PropertyInfo, PropertyInfo>> PropertyMap;

        static PropertyLister()
        {
            var b = BindingFlags.Public | BindingFlags.Instance;
            PropertyMap =
                (from f in typeof(T1).GetProperties(b)
                 join t in typeof(T2).GetProperties(b) on f.Name equals t.Name
                 select Tuple.Create(f, t))
                    .ToArray();
        }
    }
}