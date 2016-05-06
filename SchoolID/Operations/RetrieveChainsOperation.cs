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

namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    /// <summary>
    /// This class reflects the RetrieveChains operation of the Nummervoorziening service
    /// </summary>
    class RetrieveChainsOperation
    {
        private SchoolIDClient schoolIDClient;
        private RetrieveChainsRequest retrieveChainsRequest = new RetrieveChainsRequest();
        private retrieveChainsRequest1 retrieveChainsRequestWrapper = new retrieveChainsRequest1();

        /// <summary>
        /// Sets up the RetrieveChainsOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveChainsOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Sets up a RetrieveChain request, sends it through the provided School ID Client proxy class, retrieves the RetrieveChainsResponse and returns the provided Chain[].
        /// </summary>
        /// <returns>Chain[] containing active chains</returns>
        public Chain[] GetChains()
        {
            retrieveChainsRequestWrapper.retrieveChainsRequest = retrieveChainsRequest;
            retrieveChainsResponse retrieveChainsReponseWrapper = schoolIDClient.retrieveChains(retrieveChainsRequestWrapper);
            return retrieveChainsReponseWrapper.retrieveChainsResponse1;
        }
    }
}
