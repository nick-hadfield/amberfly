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
    /// Object factory configuration (fluent interface).
    /// </summary>
    public interface IConfigure
    {
        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The component type.</typeparam>
        /// <typeparam name="T">The component implementation.</typeparam>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add<I, T>() where T : I;

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The component type.</typeparam>
        /// <typeparam name="T">The component implementation.</typeparam>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add<I, T>(Instancing instancing) where T : I;

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The component type.</typeparam>
        /// <typeparam name="T">The component implementation.</typeparam>
        /// <param name="onInstantiation">The lambda to execute post instantiation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add<I, T>(Action<T> onInstantiation) where T : I;

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The component type.</typeparam>
        /// <typeparam name="T">The component implementation.</typeparam>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <param name="onInstantiation">The lambda to execute post instantiation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add<I, T>(Instancing instancing, Action<T> onInstantiation) where T : I;

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <param name="i">The service type.</param>
        /// <param name="t">The service implementation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add(Type i, Type t);

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <param name="i">The service type.</param>
        /// <param name="t">The service implementation.</param>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure Add(Type i, Type t, Instancing instancing);

        /// <summary>
        /// Registers interfaces and their matching implementations.
        /// </summary>
        IConfigure AddInterfacesAndMatchingImplementations();

        /// <summary>
        /// Registers implementation supporting the specified open generic <paramref name="gi"/>.
        /// </summary>
        /// <param name="gi">The type of the open generic interface for which we'll be finding implementations.</param>
        IConfigure AddImplementationsSupportingOpenGeneric(Type gi);
    }
}
