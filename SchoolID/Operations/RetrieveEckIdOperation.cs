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
    /// This class reflects the RetrieveEckId operation of the Nummervoorziening service
    /// </summary>
    public class RetrieveEckIdOperation
    {
        /// <summary>
        /// The SchoolID object for communication with the service
        /// </summary>
        private readonly SchoolIDClient schoolIDClient;

        /// <summary>
        /// The actual Retrieve Eck Id Request object
        /// </summary>
        private readonly RetrieveEckIdRequest retrieveEckIdRequest = new RetrieveEckIdRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveEckIdRequest1 retrieveEckIdRequestWrapper = new retrieveEckIdRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveEckIdOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveEckIdOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Provides the parameters as a RetrieveEckIdRequest to the Nummervoorziening service, fetches the RetrieveEckIdResponse and returns the ECK ID.
        /// </summary>
        /// <param name="stampseudonym">The Stampseudonym</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>The generated School ID</returns>
        public string GetEckId(string stampseudonym, string chainGuid, string sectorGuid)
        {
            Stampseudonym stampseudonymWrapped = new Stampseudonym();
            stampseudonymWrapped.Value = stampseudonym;

            this.retrieveEckIdRequest.stampseudonym = stampseudonymWrapped;
            this.retrieveEckIdRequest.chainId = chainGuid;
            this.retrieveEckIdRequest.sectorId = sectorGuid;
            this.retrieveEckIdRequestWrapper.retrieveEckIdRequest = this.retrieveEckIdRequest;

            retrieveEckIdResponse1 retrieveEckIdResponseWrapper = this.schoolIDClient.retrieveEckId(this.retrieveEckIdRequestWrapper);
            RetrieveEckIdResponse retrieveEckIdResponse = retrieveEckIdResponseWrapper.retrieveEckIdResponse;

            EckId eckId = retrieveEckIdResponse.eckId;

            return eckId.Value;
        }
    }
}
