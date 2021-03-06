﻿#region License
/*
Copyright 2016, Stichting Kennisnet

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

namespace UnitTestProject
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Chains" operation.
    /// </summary>
    [TestClass]
    public class RetrieveChainsOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that the Nummervoorziening service returns non empty list of active chains.
        /// </summary>
        [TestMethod]
        public void GetChainsTest()
        {
            Chain[] chains = EckIDServiceUtil.GetChains();

            Assert.IsNotNull(chains);
            Assert.IsTrue(chains.Length > 0);
        }
    }
}
