using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PlayTimeX.MassTransit.AppMetrics.Internal
{
    /// <summary>
    /// A set of utilities that extends Enum functionality
    /// </summary>
    internal static class EnumHelper
    {
        private static readonly ConcurrentDictionary<Type, IList> EnumCache = new ConcurrentDictionary<Type, IList>();

        /// <summary>
        /// Converts an enum into an enumerable Enum.
        /// </summary>
        /// <typeparam name="T">Any Enum type</typeparam>
        /// <returns>An IEnumerable implementation of the enum</returns>
        public static IEnumerable<T> GetEnumeration<T>(params T[] excludedValues)
        {
            CheckEnumConstraint<T>();

            if (!EnumCache.TryGetValue(typeof(T), out var list))
            {
                list = Enum.GetValues(typeof(T)).ToEnumerable<T>().ToList();
                EnumCache.TryAdd(typeof(T), list);
            }

            if (excludedValues.Any())
                return ((IEnumerable<T>)list).Except(excludedValues);
            return list as IEnumerable<T>;
        }

        /// <summary>
        /// Converts an enum into an enumerable Enum.
        /// </summary>
        /// <typeparam name="TInput">Any Enum type</typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <returns>An IEnumerable implementation of the enum</returns>
        public static IEnumerable<TOutput> GetEnumeration<TInput, TOutput>(params TInput[] excludedValues)
        {
            CheckEnumConstraint<TInput>();

            if (!EnumCache.TryGetValue(typeof(TInput), out var list))
            {
                list = Enum.GetValues(typeof(TInput)).ToEnumerable<TInput>().ToList();
                EnumCache.TryAdd(typeof(TInput), list);
            }

            if (excludedValues.Any())
                return ((IEnumerable<TInput>)list).Except(excludedValues).Cast<TOutput>();
            return ((IEnumerable<TInput>)list).Cast<TOutput>();
        }


        /// <summary>
        /// Makes sure the type is an enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private static void CheckEnumConstraint<T>()
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("MustBeEnumType");
            }
        }

        /// <summary>
        /// Makes sure the type is an enum
        /// </summary>
        /// <param name="type"></param>
        public static void CheckEnumConstraint(Type type)
        {
            if (!type.GetTypeInfo().IsEnum)
            {
                throw new ArgumentException("MustBeEnumType");
            }
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable"/> object to a strongly typed <see cref="IEnumerable&lt;T&gt;"/> object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable source)
        {
            var result = new List<T>();
            result.AddRange(source.OfType<T>());
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        public static object ChangeType(this object value, Type conversionType)
        {
            return System.Convert.ChangeType(value, conversionType);
        }
    }
}
