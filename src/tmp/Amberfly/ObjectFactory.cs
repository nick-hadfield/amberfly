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
    public class ObjectFactory : IObjectFactory
    {
        /// <summary>
        /// A singleton reference to the object factory.
        /// </summary>
        private static IObjectFactory _objectFactory = new ObjectFactory();

        /// <summary>
        /// A dictionary of component information.
        /// </summary>
        private Dictionary<Type, ComponentInfo> _componentInformation;

        /// <summary>
        /// A per-call dictionary of component information.
        /// </summary>
        private Dictionary<Type, ComponentInfo> _perCallComponentInformation;

        /// <summary>
        /// A dictionary of singleton instances, referenced by component information.
        /// </summary>
        private Dictionary<ComponentInfo, object> _singletonInstances;

        /// <summary>
        /// A dictionary of per call instances, referenced by component information.
        /// </summary>
        private Dictionary<ComponentInfo, object> _perCallInstances;

        /// <summary>
        /// Initialises a new instance of the <see cref="ObjectFactory"/> class.
        /// </summary>
        public ObjectFactory()
        {
            _componentInformation = new Dictionary<Type, ComponentInfo>();
            _singletonInstances = new Dictionary<ComponentInfo, object>();
        }

        #region Static Methods

        /// <summary>
        /// Configure the object factory.
        /// </summary>
        public static void Configure(Action<IConfigure> o)
        {
            _objectFactory.Configure(o);
        }

        /// <summary>
        /// Gets the configuration for the object factory.
        /// </summary>
        public static IConfigure Configuration
        {
            get { return _objectFactory.Configuration; }
        }

        /// <summary>
        /// Gets an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>The loaded object</returns>
        public static T Get<T>(params ComponentInfo[] components) where T : class
        {
            return _objectFactory.Get<T>(components);
        }

        /// <summary>
        /// Instantiates an object of the specified type.
        /// </summary>
        /// <param name="i">Specifies the type of the object to load.</param>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>Returns an instance of the specified type.</returns>
        public static object Get(Type i, params ComponentInfo[] components)
        {
            return _objectFactory.Get(i, components);
        }

        /// <summary>
        /// Determines if the object of type T is supported.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <returns>Returns <c>true</c> if the specified object is supported</returns>
        public static bool Supports<T>() where T : class
        {
            return _objectFactory.Supports<T>();
        }

        /// <summary>
        /// Determines if th object of the specified type is supported.
        /// </summary>
        /// <param name="i">Specifies the type of the object.</param>
        /// <returns>Returns <c>true</c> if the specified object is supported.</returns>
        public static bool Supports(Type i)
        {
            return _objectFactory.Supports(i);
        }

        #endregion

        #region IObjectFactory Methods

        /// <summary>
        /// Configure the object factory.
        /// </summary>
        void IObjectFactory.Configure(Action<IConfigure> o)
        {
            o(new Configuration(_componentInformation));
        }

        /// <summary>
        /// Gets the configuration for the object factory.
        /// </summary>
        IConfigure IObjectFactory.Configuration
        {
            get { return new Configuration(_componentInformation); }
        }

        /// <summary>
        /// Gets an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>The loaded object</returns>
        T IObjectFactory.Get<T>(params ComponentInfo[] components)
        {
            return (T)((IObjectFactory)this).Get(typeof(T), components);
        }

        /// <summary>
        /// Instantiates an object of the specified type.
        /// </summary>
        /// <param name="i">Specifies the type of the object to load.</param>
        /// <param name="components">Components available for this call to <c>Get</c> only.</param>
        /// <returns>Returns an instance of the specified type.</returns>
        object IObjectFactory.Get(Type i, params ComponentInfo[] components)
        {
            using (new PerCallScope(InitPerCallScope, TermPerCallScope))
            {
                // Make pre-created components available //
                foreach (ComponentInfo ci in components)
                {
                    _perCallComponentInformation.Add(ci.Declaration, ci);
                }

                // Was the component pre-created //
                if (_perCallComponentInformation.ContainsKey(i))
                {
                    return _perCallComponentInformation[i].Instance;
                }

                // Can we create this type? //
                if (_componentInformation.ContainsKey(i))
                {
                    ComponentInfo componentInfo = _componentInformation[i];
                    return CreateComponent(componentInfo, i);
                }

                // Can we create a generic version of this type? //
                if (_componentInformation.ContainsKey(i.GetGenericTypeDefinition()))
                {
                    ComponentInfo componentInfo = _componentInformation[i.GetGenericTypeDefinition()];
                    return CreateComponent(componentInfo, i);
                }

                // Type has not been registered. //
                throw new AmberflyException(string.Format("The specified type '{0}' has not been registered.", i));
            }
        }

        /// <summary>
        /// Determines if the object of type T is supported.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <returns>Returns <c>true</c> if the specified object is supported</returns>
        bool IObjectFactory.Supports<T>()
        {
            return ((IObjectFactory)this).Supports(typeof(T));
        }

        /// <summary>
        /// Determines if th object of the specified type is supported.
        /// </summary>
        /// <param name="i">Specifies the type of the object.</param>
        /// <returns>Returns <c>true</c> if the specified object is supported.</returns>
        bool IObjectFactory.Supports(Type i)
        {
            using (new PerCallScope(InitPerCallScope, TermPerCallScope))
            {
                // Was the component pre-created //
                if (_perCallComponentInformation.ContainsKey(i))
                {
                    return true;
                }

                // Can we create this type? //
                if (_componentInformation.ContainsKey(i))
                {
                    return true;
                }

                // Can we create a generic version of this type? //
                if (_componentInformation.ContainsKey(i.GetGenericTypeDefinition()))
                {
                    return true;
                }

                // Type has not been registered. //
                return false;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new instance of the specified component.
        /// </summary>
        /// <param name="componentInfo">Information for the component to be created.</param>
        /// <param name="i">The type to be created.</param>
        /// <returns>The created component.</returns>
        private object CreateComponent(ComponentInfo componentInfo, Type i)
        {
            object instance = null;
            ComponentInfo genericComponentInfo = null;

            // Does this component already exist? //
            switch (componentInfo.Instancing)
            {
                case LocalInstancing.Transient:
                    // Do nothing //
                    break;
                case LocalInstancing.PerCall:
                    if (_perCallInstances.ContainsKey(componentInfo))
                    {
                        return _perCallInstances[componentInfo];
                    }
                    break;
                case LocalInstancing.Singleton:
                    if (_singletonInstances.ContainsKey(componentInfo))
                    {
                        return _singletonInstances[componentInfo];
                    }
                    break;
            }

            // Create the component //
            if (componentInfo.Definition.IsGenericTypeDefinition)
            {
                //instance = CreateType(componentInfo.Definition.MakeGenericType(i.GetGenericArguments()));
                genericComponentInfo = new ComponentInfo(i, componentInfo.Definition.MakeGenericType(i.GetGenericArguments()), componentInfo.Instancing.Cast<Instancing>());
                instance = genericComponentInfo.Instantiate();
            }
            else
            {
                //instance = CreateType(componentInfo.Definition);
                instance = componentInfo.Instantiate();
            }

            // Bale if component is null //
            if (null == instance) return null;

            // Does this component need to be stored for later use? //
            switch (componentInfo.Instancing)
            {
                case LocalInstancing.Transient:
                    // Do nothing //
                    break;
                case LocalInstancing.PerCall:
                    if (componentInfo.Definition.IsGenericTypeDefinition)
                    {
                        //ComponentInfo genericComponentInfo = new ComponentInfo(i, instance.GetType(), componentInfo.Instancing);
                        _componentInformation.Add(i, genericComponentInfo);
                        _perCallInstances.Add(genericComponentInfo, instance);
                    }
                    else
                    {
                        if (!_perCallInstances.ContainsKey(componentInfo))
                        {
                            _perCallInstances.Add(componentInfo, instance);
                        }
                    }
                    break;
                case LocalInstancing.Singleton:
                    if (componentInfo.Definition.IsGenericTypeDefinition)
                    {
                        //ComponentInfo genericComponentInfo = new ComponentInfo(i, instance.GetType(), componentInfo.Instancing);
                        _componentInformation.Add(i, genericComponentInfo);
                        _perCallInstances.Add(genericComponentInfo, instance);

                    }
                    else
                    {
                        if (!_singletonInstances.ContainsKey(componentInfo))
                        {
                            _singletonInstances.Add(componentInfo, instance);
                        }
                    }
                    break;
            }

            // Return the instance //
            return instance;
        }

        /// <summary>
        /// Used to initialise dictionaries used to store supplied components.
        /// </summary>
        private void InitPerCallScope()
        {
            if (null == _perCallInstances)
            {
                _perCallInstances = new Dictionary<ComponentInfo, object>();
                _perCallComponentInformation = new Dictionary<Type, ComponentInfo>();
            }
        }

        /// <summary>
        /// Used to terminate/cleanup dictionaries used to store supplied components.
        /// </summary>
        private void TermPerCallScope()
        {
            _perCallComponentInformation = null;
            _perCallInstances = null;
        }

        #endregion
    }
}
