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
    /// Configuration information for object factory.
    /// </summary>
    public class Configuration : IConfigure
    {
        /// <summary>
        /// A dictionary of component information.
        /// </summary>
        private Dictionary<Type, ComponentInfo> _componentInformation;

        /// <summary>
        /// Initialises a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        /// <param name="componentInformation"></param>
        internal Configuration(Dictionary<Type, ComponentInfo> componentInformation)
        {
            _componentInformation = Enforce.NotNull(componentInformation, "componentInformation");
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The service type.</typeparam>
        /// <typeparam name="T">The service implementation.</typeparam>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add<I, T>()
        {
            return ((IConfigure)this).Add<I, T>(Instancing.PerCall);
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The service type.</typeparam>
        /// <typeparam name="T">The service implementation.</typeparam>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add<I, T>(Instancing instancing)
        {
            if (_componentInformation.ContainsKey(typeof(I)))
            {
                throw new AmberflyException(string.Format("Type is already in dictionary. {0}", typeof(I)));
            }
            else
            {
                _componentInformation.Add(typeof(I), new ComponentInfo<I, T>(instancing));
            }
            return this;
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The service type.</typeparam>
        /// <typeparam name="T">The service implementation.</typeparam>
        /// <param name="onInstantiation">The lambda to execute post instantiation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add<I, T>(Action<T> onInstantiation)
        {
            return ((IConfigure)this).Add<I, T>(Instancing.PerCall, onInstantiation);
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <typeparam name="I">The service type.</typeparam>
        /// <typeparam name="T">The service implementation.</typeparam>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <param name="onInstantiation">The lambda to execute post instantiation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add<I, T>(Instancing instancing, Action<T> onInstantiation)
        {
            if (_componentInformation.ContainsKey(typeof(I)))
            {
                throw new AmberflyException(string.Format("Type is already in dictionary. {0}", typeof(I)));
            }
            else
            {
                _componentInformation.Add(typeof(I), new ComponentInfo<I, T>(instancing, onInstantiation));
            }
            return this;
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <param name="i">The service type.</param>
        /// <param name="t">The service implementation.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add(Type i, Type t)
        {
            return ((IConfigure)this).Add(i, t, Instancing.PerCall);
        }

        /// <summary>
        /// Registers a new service with the container.
        /// </summary>
        /// <param name="i">The service type.</param>
        /// <param name="t">The service implementation.</param>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <returns>The object factory (fluent interface).</returns>
        IConfigure IConfigure.Add(Type i, Type t, Instancing instancing)
        {
            if (_componentInformation.ContainsKey(i))
            {
                // throw new BarracudaException(string.Format("Type is already in dictionary. {0}", i));
            }
            else
            {
                _componentInformation.Add(i, new ComponentInfo(i, t, instancing));
            }
            return this;
        }

        IConfigure IConfigure.AddInterfacesAndMatchingImplementations()
        {
            Extensions.GetInterfacesWithImplementations().ForEach(
                (i) => ((IConfigure)this).Add(i, i.FindImplementation())
            );
            return this;
        }

        IConfigure IConfigure.AddImplementationsSupportingOpenGeneric(Type gi)
        {
            // IHandler<REQUEST> //
            Extensions.GetImplementationsSupportingOpenGeneric(gi).ForEach(
                (t) => ((IConfigure)this).Add(t.GetInterfaceSupportingOpenGeneric(gi), t)
            );
            return this;
        }
    }
}
