namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    /// <summary>
    /// This class reflects the RetrieveEckId operation of the Nummervoorziening service
    /// </summary>
    class RetrieveEckIdOperation
    {
        private SchoolIDClient schoolIDClient;
        private RetrieveEckIdRequest retrieveEckIdRequest = new RetrieveEckIdRequest();
        private retrieveEckIdRequest1 retrieveEckIdRequestWrapper = new retrieveEckIdRequest1();

        /// <summary>
        /// Sets up the RetrieveEckIdOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveEckIdOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Provides the parameters as a RetrieveEckIdRequest to the Nummervoorziening service, fetches the RetrieveEckIdResponse and returns the ECK ID.
        /// </summary>
        /// <param name="input">The scrypt hashed PGN</param>
        /// <param name="chainGuid">A valid chain id</param>
        /// <param name="sectorGuid">A valid sector id</param>
        /// <returns>The generated School ID</returns>
        public string GetEckId(string input, string chainGuid, string sectorGuid)
        {
            HPgn hpgn = new HPgn();
            hpgn.Value = input;

            retrieveEckIdRequest.hpgn = hpgn;
            retrieveEckIdRequest.chainId = chainGuid;
            retrieveEckIdRequest.sectorId = sectorGuid;
            retrieveEckIdRequestWrapper.retrieveEckIdRequest = retrieveEckIdRequest;

            retrieveEckIdResponse1 retrieveEckIdResponseWrapper = schoolIDClient.retrieveEckId(retrieveEckIdRequestWrapper);
            RetrieveEckIdResponse retrieveEckIdResponse = retrieveEckIdResponseWrapper.retrieveEckIdResponse;

            EckId eckId = retrieveEckIdResponse.eckId;

            return eckId.Value;
        }
    }
}
