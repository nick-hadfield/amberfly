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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Amberfly.Test
{
    /// <summary>
    /// Summary description for ObjectFactoryTest
    /// </summary>
    [TestClass]
    public class ObjectFactoryTest
    {
        [TestMethod]
        public void CanInstantiateNonSpecificGenericTypeDynamically()
        {
            IObjectFactory objectFactory = new ObjectFactory();
            objectFactory.Configuration.Add(typeof(IList<>), typeof(List<>));

            IList<string> list = objectFactory.Get<IList<string>>();

            list.Add("Hello");
            list.Add("World");
        }

        [TestMethod]
        public void CanInstantiateNonSpecificGenericTypeStatically()
        {
            IList<string> list = null;
            ObjectFactory.Configuration.Add(typeof(IList<>), typeof(List<>));
            list = ObjectFactory.Get<IList<string>>();
            list.Add("Hello");
            list.Add("World");
        }

        [TestMethod]
        public void CanInstantiateSpecificGenericTypeMethodDynamically()
        {
            IObjectFactory objectFactory = new ObjectFactory();
            objectFactory.Configuration.Add(typeof(IList<>), typeof(List<>));
            objectFactory.Configuration.Add<IList<string>, List<string>>();

            IList<string> list = objectFactory.Get<IList<string>>();

            list.Add("Hello");
            list.Add("World");
        }
    }
}
