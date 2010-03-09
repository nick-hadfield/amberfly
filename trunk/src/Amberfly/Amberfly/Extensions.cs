#region License
//
// Copyright 2009 Nicholas Hadfield
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
//
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amberfly
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Casts from one enumeration to another.
        /// </summary>
        /// <typeparam name="E">The enumeration to cast to.</typeparam>
        /// <param name="e">The enumeration to be casted.</param>
        /// <returns>The target enumeration value.</returns>
        public static E Cast<E>(this Enum e)
        {
            return (E)Enum.Parse(typeof(E), e.ToString());
        }

        #region Functional

        /// <summary>
        /// Make a value enumerable.
        /// </summary>
        /// <typeparam name="T">The type we of the value we want to make enumberable.</typeparam>
        /// <param name="value">The value we want to make enumerable.</param>
        /// <returns>An enumeration containing the specified value.</returns>
        public static IEnumerable<T> Yield<T>(this T value)
        {
            yield return value;
        }

        /// <summary>
        /// This does the same thing that linq's <c>select</c> extension method does.
        /// </summary>
        public static IEnumerable<T> Map<T>(this IEnumerable<T> values, Func<T, T> f)
        {
            foreach (T item in values)
            {
                yield return f(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Func<T, T> On<T>(this Func<T, T> g, Func<T, T> f)
        {
            return t => f(g(t));
        }

        /// <summary>
        /// Executes the specified <paramref name="action"/> for each member of the enumeration.
        /// </summary>
        /// <param name="values">The values to be enumerated.</param>
        /// <param name="action">The action to execute for each value in the enumeration.</param>
        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            Enforce.NotNull(action);
            foreach (T item in values)
            {
                action(item);
            }
        }

        #endregion

        #region Configuration

        /// <summary>
        /// Returns types available in the entry assembly.
        /// </summary>
        /// <remarks>
        /// This should probably be enhances so that types are passed to the extension methods instead.
        /// </remarks>
        public static IEnumerable<Type> Types
        {
            get
            {
                return System.Reflection.Assembly.GetEntryAssembly().GetTypes();
            }
        }

        /// <summary>
        /// Fina a type the implements the specified interface.
        /// </summary>
        public static Type FindImplementation(this Type i)
        {
            if (!i.IsInterface) throw new ArgumentException("i must be an interface.", "i");
            if (!i.Name.StartsWith("I")) return null;

            var types = from t in Types
                        where true
                             && t.Name == i.Name.TrimStart(new char[] { 'I' })
                             && i.IsAssignableFrom(t)
                        select t;

            return types.FirstOrDefault();
        }

        /// <summary>
        /// Get interfaces that have implementations.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetInterfacesWithImplementations()
        {
            return from t in Types
                   where t.IsInterface && null != t.FindImplementation()
                   select t;
        }

        /// <summary>
        /// Get implementations that support the specified open generic.
        /// </summary>
        public static IEnumerable<Type> GetImplementationsSupportingOpenGeneric(Type gi)
        {
            return from t in Types
                   where null != t.GetInterfaceSupportingOpenGeneric(gi)
                   select t;
        }

        /// <summary>
        /// Get an interface for the specified open generic.
        /// </summary>
        public static Type GetInterfaceSupportingOpenGeneric(this Type t, Type gi)
        {
            if (gi.IsGenericTypeDefinition)
            {
                foreach (var i in t.GetInterfaces())
                {
                    if (i.IsGenericType)
                    {
                        if (i.GetGenericTypeDefinition() == gi)
                        {
                            return i;
                        }
                    }
                }
            }
            return null;
        }

        #endregion

    }
}
