#region License
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NVA_DotNetReferenceImplementation.SchoolID.Operations;

namespace UnitTestProject
{
    /// <summary>
    /// Demonstrates correct usage of the "Retrieve Sectors" operation.
    /// </summary>
    [TestClass]
    public class RetrieveSectorsOperationUnitTest : AbstractUnitTest
    {
        /// <summary>
        /// Tests that the Nummervoorziening service returns non empty list of active sectors.
        /// </summary>
        [TestMethod]
        public void GetSectorsTest()
        {
            Sector[] sectors = schoolIDServiceUtil.GetSectors();

            Assert.IsNotNull(sectors);
            Assert.IsTrue(sectors.Length > 0);
        }
    }
}
