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

namespace EckID.Operations
{
    /// <summary>
    /// This class reflects the RetrieveChains operation of the Nummervoorziening service
    /// </summary>
    public class RetrieveChainsOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private readonly EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Retrieve Chains Request object
        /// </summary>
        private readonly RetrieveChainsRequest _retrieveChainsRequest = new RetrieveChainsRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveChainsRequest1 _retrieveChainsRequestWrapper = new retrieveChainsRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveChainsOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public RetrieveChainsOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Sets up a RetrieveChain request, sends it through the provided School ID Client proxy class, retrieves the RetrieveChainsResponse and returns the provided Chain[].
        /// </summary>
        /// <returns>Chain[] containing active chains</returns>
        public Chain[] GetChains()
        {
            _retrieveChainsRequestWrapper.retrieveChainsRequest = _retrieveChainsRequest;
            retrieveChainsResponse retrieveChainsReponseWrapper = _eckIdClient.retrieveChains(_retrieveChainsRequestWrapper);
            return retrieveChainsReponseWrapper.retrieveChainsResponse1;
        }
    }
}
