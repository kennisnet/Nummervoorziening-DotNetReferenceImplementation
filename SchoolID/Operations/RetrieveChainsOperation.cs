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
