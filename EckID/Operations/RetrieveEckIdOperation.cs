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
    /// This class reflects the RetrieveEckID operation of the Nummervoorziening service
    /// </summary>
    public class RetrieveEckIDOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private readonly EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Retrieve Eck Id Request object
        /// </summary>
        private readonly RetrieveEckIdRequest _retrieveEckIDRequest = new RetrieveEckIdRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveEckIdRequest1 _retrieveEckIDRequestWrapper = new retrieveEckIdRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveEckIDOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public RetrieveEckIDOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Provides the parameters as a RetrieveEckIDRequest to the Nummervoorziening service, fetches the RetrieveEckIDResponse and returns the ECK ID.
        /// </summary>
        /// <param name="stampseudonym">The Stampseudonym</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>The generated School ID</returns>
        public string GetEckID(string stampseudonym, string chainGuid, string sectorGuid)
        {
            Stampseudonym stampseudonymWrapped = new Stampseudonym();
            stampseudonymWrapped.Value = stampseudonym;

            _retrieveEckIDRequest.stampseudonym = stampseudonymWrapped;
            _retrieveEckIDRequest.chainId = chainGuid;
            _retrieveEckIDRequest.sectorId = sectorGuid;
            _retrieveEckIDRequestWrapper.retrieveEckIdRequest = _retrieveEckIDRequest;

            retrieveEckIdResponse1 retrieveEckIDResponseWrapper = _eckIdClient.retrieveEckId(_retrieveEckIDRequestWrapper);
            RetrieveEckIdResponse retrieveEckIDResponse = retrieveEckIDResponseWrapper.retrieveEckIdResponse;

            EckId eckId = retrieveEckIDResponse.eckId;

            return eckId.Value;
        }
    }
}
