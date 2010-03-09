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
using System.Reflection;

namespace Amberfly
{
    /// <summary>
    /// Defines a component for use by the object factory.
    /// </summary>
    public class ComponentInfo
    {
        /// <summary>
        /// The declaration <c>Type</c> that determines when this component should be instantiated.
        /// </summary>
        private Type _declaration;

        /// <summary>
        /// Gets the declaration <c>Type</c> that determines when this component should be instantiated.
        /// </summary>
        public Type Declaration
        {
            get { return _declaration; }
        }

        /// <summary>
        /// The definition <c>Type</c> to be instantiated for this component.
        /// </summary>
        private Type _definition;

        /// <summary>
        /// Gets the definition <c>Type</c> to be instantiated for this component.
        /// </summary>
        public Type Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Determines how the component should be instanced.
        /// </summary>
        private LocalInstancing _instancing;

        /// <summary>
        /// Gets a value determing how the component should be instanced.
        /// </summary>
        internal LocalInstancing Instancing
        {
            get { return _instancing; }
        }

        /// <summary>
        /// An instance of this component.
        /// </summary>
        private object _instance;

        /// <summary>
        /// Gets an instance of this component.
        /// </summary>
        public object Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        /// <summary>
        /// Instantiate the type.
        /// </summary>
        /// <returns>An instance of the type.</returns>
        public virtual object Instantiate()
        {
            ConstructorInfo[] constructors = Definition.GetConstructors();
            if (constructors.Length == 0)
            {
                throw new AmberflyException(string.Format("Public constructor required in order to satisfy dependencies '{0}'.", Definition));
            }

            foreach (ConstructorInfo constructorInfo in Definition.GetConstructors())
            {
                if (CanSatisfyConstructor(constructorInfo))
                {
                    object[] arguments = SatisfyConstructorArguments(constructorInfo);
                    return System.Activator.CreateInstance(Definition, arguments);
                }
            }

            throw new AmberflyException(string.Format("Dependencies could not be satisfied '{0}'.", Declaration));
        }

        /// <summary>
        /// Returns true if constructor can be instantiated.
        /// </summary>
        /// <param name="constructorInfo">The constructor.</param>
        /// <returns>Returns <c>true</c> if constractor can be satisfied.</returns>
        private bool CanSatisfyConstructor(ConstructorInfo constructorInfo)
        {
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                if (!ObjectFactory.Supports(parameterInfo.ParameterType))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the constructor arguments.
        /// </summary>
        /// <param name="constructorInfo">The constructor whose arguments are needed.</param>
        /// <returns>The array of arguments required by the constructor.</returns>
        private object[] SatisfyConstructorArguments(ConstructorInfo constructorInfo)
        {
            int i = 0;
            object[] arguments = new object[constructorInfo.GetParameters().Count()];
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                arguments[i++] = ObjectFactory.Get(parameterInfo.ParameterType);
            }
            return arguments;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="declaration">The declaration <c>Type</c> that determines when this component should be instantiated.</param>
        /// <param name="instance">An instance of the component.</param>
        public ComponentInfo(Type declaration, object instance)
        {
            _declaration = Enforce.NotNull(declaration);
            _definition = instance.GetType();
            _instancing = LocalInstancing.Supplied;
            _instance = Enforce.NotNull(instance, "instance");
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="declaration">The declaration <c>Type</c> that determines when this component should be instantiated.</param>
        /// <param name="definition">The definition <c>Type</c> to be instantiated for this component.</param>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        internal ComponentInfo(Type declaration, Type definition, Instancing instancing)
        {
            _declaration = Enforce.NotNull(declaration);
            _definition = Enforce.NotNull(definition);
            _instancing = Enforce.NotNull(instancing).Cast<LocalInstancing>();
        }
    }

    /// <summary>
    /// Defines a component for use by the object factory.
    /// </summary>
    /// <typeparam name="DECLARATION"></typeparam>
    public class ComponentInfo<DECLARATION> : ComponentInfo
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="instance">An instance of the component.</param>
        public ComponentInfo(object instance) : base(typeof(DECLARATION), instance) { }
    }

    /// <summary>
    /// Defines a component for use by the object factory.
    /// </summary>
    /// <typeparam name="DECLARATION">The declaration <c>Type</c> that determines when this component should be instantiated.</typeparam>
    /// <typeparam name="DEFINITION">The definition <c>Type</c> to be instantiated for this component.</typeparam>
    public class ComponentInfo<DECLARATION, DEFINITION> : ComponentInfo where DEFINITION : DECLARATION
    {
        /// <summary>
        /// An action to be applied to the instance immediately after instantiation.
        /// </summary>
        private Action<DEFINITION> _onInstantiation = null;

        /// <summary>
        /// Override the <see cref="Instantiate"/> method in order to apply the lambda.
        /// </summary>
        /// <returns>An instance of the type.</returns>
        public override object Instantiate()
        {
            DEFINITION instance = (DEFINITION)base.Instantiate();
            if (null != _onInstantiation)
            {
                _onInstantiation(instance);
            }
            return (object)instance;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        internal ComponentInfo(Instancing instancing)
            : base(typeof(DECLARATION), typeof(DEFINITION), instancing)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ComponentInfo"/> class.
        /// </summary>
        /// <param name="instancing">Determines how the component should be instanced.</param>
        /// <param name="onInstantiation">A lambda to be after instantiation.</param>
        internal ComponentInfo(Instancing instancing, Action<DEFINITION> onInstantiation)
            : base(typeof(DECLARATION), typeof(DEFINITION), instancing)
        {
            _onInstantiation = Enforce.NotNull(onInstantiation, "onInstantiation");
        }
    }
}
