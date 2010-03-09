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
    /// The per call scope, manages initialisation/termination of the per call dictionary.
    /// </summary>
    internal class PerCallScope : IDisposable
    {
        /// <summary>
        /// To be called if this is the last scope.
        /// </summary>
        private Action _term;

        /// <summary>
        /// Initialise a new instance of the <see cref="PerCallScope"/> class.
        /// </summary>
        /// <param name="init"></param>
        /// <param name="term"></param>
        public PerCallScope(Action init, Action term)
        {
            new Scopes.ScopeStack<PerCallScope>("").Push(this);
            _term = Enforce.NotNull(term, "term");
            init();
        }

        /// <summary>
        /// Call the term delegate if this is the last scope.
        /// </summary>
        void IDisposable.Dispose()
        {
            new Scopes.ScopeStack<PerCallScope>("").Pop();
            if (null == new Scopes.ScopeStack<PerCallScope>("").Peek())
            {
                _term();
            }
        }
    }
}
