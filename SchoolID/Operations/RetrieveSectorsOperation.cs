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
    /// This class reflects the RetrieveSectors operation of the Nummervoorziening service
    /// </summary>
    public class RetrieveSectorsOperation
    {
        /// <summary>
        /// The SchoolID object for communication with the service
        /// </summary>
        private readonly SchoolIDClient schoolIDClient;

        /// <summary>
        /// The actual Retrieve Sectors Request object
        /// </summary>
        private readonly RetrieveSectorsRequest retrieveSectorsRequest = new RetrieveSectorsRequest();

        /// <summary>
        /// The wrapper class containing the request to be send to the service
        /// </summary>
        private readonly retrieveSectorsRequest1 retrieveSectorsRequestWrapper = new retrieveSectorsRequest1();

        /// <summary>
        /// Initializes a new instance of the <see cref="RetrieveSectorsOperation" /> class with a reference to the ShoolIDClient proxy class
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
            this.retrieveSectorsRequestWrapper.retrieveSectorsRequest = this.retrieveSectorsRequest;
            retrieveSectorsResponse retrieveSectorsReponseWrapper = this.schoolIDClient.retrieveSectors(this.retrieveSectorsRequestWrapper);
            return retrieveSectorsReponseWrapper.retrieveSectorsResponse1;
        }
    }
}
