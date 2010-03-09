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
    /// Determines how how a component should be instanced/scoped.
    /// </summary>
    internal enum LocalInstancing
    {
        /// <summary>
        /// If a component is requested twice in one <c>Get</c> the same component will be returned.
        /// </summary>
        /// <remarks>A thread static scope is used internally to maintain instances for the lifetime of the <c>Get</c>/</remarks>
        PerCall,

        /// <summary>
        /// A new instance is returned every time.
        /// </summary>
        Transient,

        /// <summary>
        /// The same instance is returned every time for the lifetime of the application.
        /// </summary>
        Singleton,

        /// <summary>
        /// The component is supplied when <c>Get</c> is called.
        /// </summary>
        Supplied
    }
}
