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
    /// Validation utilities.
    /// </summary>
    public static class Enforce
    {
        #region NotNull

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <returns>The parameter.</returns>
        public static object NotNull(object value)
        {
            if (null == value) new ArgumentNullException();
            return value;
        }

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <param name="name">The name of the parameter being validated.</param>
        /// <returns>The parameter.</returns>
        public static object NotNull(object value, string name)
        {
            if (null == value) new ArgumentNullException(name);
            return value;
        }

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <returns>The parameter.</returns>
        public static T NotNull<T>(T value)
        {
            if (null == value) throw new ArgumentNullException();
            return value;
        }

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <param name="name">The name of the parameter being validated.</param>
        /// <returns>The parameter.</returns>
        public static T NotNull<T>(T value, string name)
        {
            if (null == value) throw new ArgumentNullException(name);
            return value;
        }

        #endregion

        #region NotEmpty

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <returns>The parameter.</returns>
        public static string NotEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AmberflyException(TextResource.StringShouldNotBeEmpty);
            }
            return value;
        }

        /// <summary>
        /// Throws an exception if <paramref name="value"/> parameter is <c>null</c>.
        /// </summary>
        /// <param name="value">The parameter to be validated.</param>
        /// <param name="name">The name of the parameter being validated.</param>
        /// <returns>The parameter.</returns>
        public static string NotEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AmberflyException(string.Format(TextResource.SpecificStringShouldNotBeEmpty, name));
            }
            return value;
        }

        #endregion
    }
}
