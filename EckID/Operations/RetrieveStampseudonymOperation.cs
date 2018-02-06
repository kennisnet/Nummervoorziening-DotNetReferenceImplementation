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
    public class RetrieveStampseudonymOperation
    {
        /// <summary>
        /// The EckID object for communication with the service
        /// </summary>
        private readonly EckIDPortClient _eckIdClient;

        /// <summary>
        /// The actual Retrieve Stampseudonym Request object
        /// </summary>
        private readonly RetrieveStampseudonymRequest _retrieveStampseudonymRequest = new RetrieveStampseudonymRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveStampseudonymRequest1 _retrieveStampseudonymRequestWrapper = new retrieveStampseudonymRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveStampseudonymOperation" /> class with a reference to the ShoolIDClient proxy class
        /// </summary>
        /// <param name="eckIdClient">An initialized EckIDPortClient proxy class</param>
        public RetrieveStampseudonymOperation(EckIDPortClient eckIdClient)
        {
            _eckIdClient = eckIdClient;
        }

        /// <summary>
        /// Provides the parameters as a RetrieveEckIDRequest to the Nummervoorziening service, fetches the RetrieveStampseudonymResponse and returns the Stampseudonym.
        /// </summary>
        /// <param name="hpgn">The scrypt hashed PGN</param>
        /// <returns>The generated Stampseudonym</returns>
        public string GetStampseudonym(string hpgn)
        {
            HPgn hpgnWrapper = new HPgn { Value = hpgn };

            _retrieveStampseudonymRequest.hpgn = hpgnWrapper;
            _retrieveStampseudonymRequestWrapper.retrieveStampseudonymRequest = _retrieveStampseudonymRequest;

            retrieveStampseudonymResponse1 retrieveStampseudonymResponseWrapper =
                _eckIdClient.retrieveStampseudonym(_retrieveStampseudonymRequestWrapper);

            RetrieveStampseudonymResponse retrieveStampseudonymResponse = retrieveStampseudonymResponseWrapper.retrieveStampseudonymResponse;

            Stampseudonym stampseudonym = retrieveStampseudonymResponse.stampseudonym;

            return stampseudonym.Value;
        }
    }
}
