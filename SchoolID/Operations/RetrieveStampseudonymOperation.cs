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
    public class RetrieveStampseudonymOperation
    {
        /// <summary>
        /// The SchoolID object for communication with the service
        /// </summary>
        private readonly SchoolIDClient schoolIDClient;

        /// <summary>
        /// The actual Retrieve Stampseudonym Request object
        /// </summary>
        private readonly RetrieveStampseudonymRequest retrieveStampseudonymRequest = new RetrieveStampseudonymRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveStampseudonymRequest1 retrieveStampseudonymRequestWrapper = new retrieveStampseudonymRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveStampseudonymOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveStampseudonymOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Provides the parameters as a RetrieveEckIdRequest to the Nummervoorziening service, fetches the RetrieveStampseudonymResponse and returns the Stampseudonym.
        /// </summary>
        /// <param name="hpgn">The scrypt hashed PGN</param>
        /// <returns>The generated Stampseudonym</returns>
        public string GetStampseudonym(string hpgn)
        {
            HPgn hpgnWrapper = new HPgn { Value = hpgn };

            this.retrieveStampseudonymRequest.hpgn = hpgnWrapper;
            this.retrieveStampseudonymRequestWrapper.retrieveStampseudonymRequest = this.retrieveStampseudonymRequest;

            retrieveStampseudonymResponse1 retrieveStampseudonymResponseWrapper = 
                this.schoolIDClient.retrieveStampseudonym(this.retrieveStampseudonymRequestWrapper);

            RetrieveStampseudonymResponse retrieveStampseudonymResponse = retrieveStampseudonymResponseWrapper.retrieveStampseudonymResponse;

            Stampseudonym stampseudonym = retrieveStampseudonymResponse.stampseudonym;

            return stampseudonym.Value;
        }
    }
}
