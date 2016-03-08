namespace NVA_DotNetReferenceImplementation.SchoolID.Operations
{
    /// <summary>
    /// This class reflects the RetrieveSectors operation of the Nummervoorziening service
    /// </summary>
    class RetrieveSectorsOperation
    {
        private SchoolIDClient schoolIDClient;
        private RetrieveSectorsRequest retrieveSectorsRequest = new RetrieveSectorsRequest();
        private retrieveSectorsRequest1 retrieveSectorsRequestWrapper = new retrieveSectorsRequest1();

        /// <summary>
        /// Sets up the RetrieveSectorsOperation object with a reference to the SchoolIDClient proxy class
        /// </summary>
        /// <param name="schoolIDClient">An initialized SchoolIDClient proxy class</param>
        public RetrieveSectorsOperation(SchoolIDClient schoolIDClient)
        {
            this.schoolIDClient = schoolIDClient;
        }

        /// <summary>
        /// Sets up a RetrieveSector request, sends it through the provided School ID Client proxy class, retrieves the RetrieveSectorsResponse and returns the provided Sector[].
        /// </summary>
        /// <returns>Sector[] containing active sectors</returns>
        public Sector[] GetSectors()
        {
            retrieveSectorsRequestWrapper.retrieveSectorsRequest = retrieveSectorsRequest;
            retrieveSectorsResponse retrieveSectorsReponseWrapper = schoolIDClient.retrieveSectors(retrieveSectorsRequestWrapper);
            return retrieveSectorsReponseWrapper.retrieveSectorsResponse1;
        }
    }
}
