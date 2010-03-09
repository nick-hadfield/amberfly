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
    /// The object factory.
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        /// Configure the object factory.
        /// </summary>
        /// <param name="o"></param>
        void Configure(Action<IConfigure> o);

        /// <summary>
        /// Gets the configuration for the object factory.
        /// </summary>
        IConfigure Configuration { get; }

        /// <summary>
        /// Gets an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>The loaded object</returns>
        T Get<T>(params ComponentInfo[] components) where T : class;

        /// <summary>
        /// Instantiates an object of the specified type.
        /// </summary>
        /// <param name="type">Specifies the type of the object to load.</param>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>Returns an instance of the specified type.</returns>
        object Get(Type type, params ComponentInfo[] components);

        /// <summary>
        /// Determines if the object of type T is supported.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <returns>Returns <c>true</c> if the specified object is supported</returns>
        bool Supports<T>() where T : class;

        /// <summary>
        /// Determines if th object of the specified type is supported.
        /// </summary>
        /// <param name="type">Specifies the type of the object.</param>
        /// <returns>Returns <c>true</c> if the specified object is supported.</returns>
        bool Supports(Type type);
    }
}
