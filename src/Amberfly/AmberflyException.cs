﻿#region License
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
using System.Runtime.Serialization;

namespace Amberfly
{
    /// <summary>
    /// Represents errors that occur during framework execution.
    /// </summary>
    [Serializable]
    public class AmberflyException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AmberflyException" /> class.
        /// </summary>
        public AmberflyException() : base() { }

        /// <summary>
        /// Initialises a new instance of the <see cref="AmberflyException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AmberflyException(string message) : base(message) { }

        /// <summary>
        /// Initialises a new instance of the <see cref="AmberflyException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AmberflyException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initialises a new instance of the <see cref="AmberflyException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialised object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination. </param>
        protected AmberflyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
